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

namespace EmailWorker
{
    public class Program
    {
        private const string _sectionKey = "Gmail";
        private const string _queue = "queue-mail";
        private static string _host;
        private static string _password;
        private static string _username;
        private static string _path;

        public static void Main(string[] args)
        {
            _path = Environment.GetEnvironmentVariable("LOGGER_PATH");           
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(_path)
                .CreateLogger();

            var configuration = CreateConfiguratuion();
            configuration.SetEnvironmentVariableForConfiguration();
            CreateHostBuilder(args, configuration).Build().Run();
        }

        public static IConfiguration CreateConfiguratuion() =>
            new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                      .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                                      .Build();

        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices(services =>
                {
                    services.AddOptions<EmailConfig>().Bind(configuration.GetSection(_sectionKey));
                    services.AddOptions<RabbitMQConfig>().Bind(configuration.GetSection("RabbitMQ"));
                    services.AddTransient<IEMailSenderService, EMailSenderService>();
                    services.AddHostedService<Worker>();

                    _host = Environment.GetEnvironmentVariable("RABBITMQ_HOST");
                    _username = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME");
                    _password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");

                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<MailConsumer>();
                        x.SetKebabCaseEndpointNameFormatter();
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host(_host, h =>
                            {
                                h.Username(_username);
                                h.Password(_password);
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