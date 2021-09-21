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
        private const string _gmailSectionKey = "Gmail";
        private const string _rabbirMqSectionKey = "RabbitMQ";
        private const string _queue = "queue-mail";
        private static string _rabbitHost = "Host";
        private static string _rabbitPassword = "Password";
        private static string _rabbitUsername = "Username";
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
                    services.AddOptions<EmailConfig>().Bind(configuration.GetSection(_gmailSectionKey));
                    services.AddOptions<RabbitMq>().Bind(configuration.GetSection(_rabbirMqSectionKey));
                    services.AddTransient<IEMailSenderService, EMailSenderService>();
                    services.AddHostedService<Worker>();

                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<MailConsumer>();
                        x.SetKebabCaseEndpointNameFormatter();
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host(configuration.GetValue<string>($"{_rabbirMqSectionKey}:{_rabbitHost}"), h =>
                            {
                                h.Username(configuration.GetValue<string>($"{_rabbirMqSectionKey}:{_rabbitUsername}"));
                                h.Password(configuration.GetValue<string>($"{_rabbirMqSectionKey}:{_rabbitPassword}"));
                                
                                Log.Information($"Host:{_rabbirMqSectionKey}:{_rabbitHost}");
                                Log.Information($"Username:{_rabbirMqSectionKey}:{_rabbitUsername}");
                                Log.Information($"Password:{_rabbirMqSectionKey}:{_rabbitPassword}");
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