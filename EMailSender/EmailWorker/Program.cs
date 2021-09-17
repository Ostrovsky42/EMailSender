using System;
using System.IO;
using EmailWorker.Extentions;
using EmailWorker.Service;
using EmailWorker.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using static EmailWorker.EnviromentChecker;

namespace EmailWorker
{
    public class Program
    {
        private const string _sectionKey = "Gmail";
        private const string _queue = "queue-mail";
        private static string _rabbitHost = "RABBITMQ_HOST";
        private static string _rabbitPassword = "RABBITMQ_PASSWORD";
        private static string _rabbitUsername = "RABBITMQ_USERNAME";
        private static string _loggerPath = "LOGGER_PATH";
        private static string _defaultLoggerPath = "C:\\Services\\EmailSender\\Logs.txt";

        public static void Main(string[] args)
        {
            var path = Environment.GetEnvironmentVariable(_loggerPath) ?? _defaultLoggerPath;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(path, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var configuration = CreateConfiguratuion();
            configuration.SetEnvironmentVariableForConfiguration();
            CreateHostBuilder(args, configuration).Build().Run();
        }

        public static IConfiguration CreateConfiguratuion() =>
            new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                      .Build();

        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices(services =>
                {
                    services.AddOptions<EmailConfig>().Bind(configuration.GetSection(_sectionKey));
                    services.AddTransient<IEMailSenderService, EMailSenderService>();
                    services.AddHostedService<Worker>();

                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<MailConsumer>();
                        x.SetKebabCaseEndpointNameFormatter();
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host(GetkAndLogEnviromentVariable(_rabbitHost), h =>
                            {
                                h.Username(GetkAndLogEnviromentVariable(_rabbitUsername));
                                h.Password(GetkAndLogEnviromentVariable(_rabbitPassword));
                            });
                            cfg.ReceiveEndpoint(_queue, e =>
                            {
                                e.ConfigureConsumer<MailConsumer>(context);
                            });
                        });
                    });

                    services.AddMassTransitHostedService();
                });
    }
}