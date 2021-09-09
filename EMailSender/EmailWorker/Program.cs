using System;
using System.IO;
using System.Threading.Tasks;
using EmailWorker;
using EmailWorker.Extensions;
using EmailWorker.Models;
using EmailWorker.Service;
using EmailWorker.Settings;
using EventContracts;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspNetCoreListener
{
    public class Program
    {
        private const string _queue = "queue-mail";
        private const string _sectionKey = "Gmail";

        public static void Main(string[] args)
        {
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
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions<EmailConfig>().Bind(configuration.GetSection(_sectionKey));
                    services.AddTransient<IEMailSenderService, EMailSenderService>();
                    services.AddHostedService<Worker>();

                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<EmailConsumer>();
                        x.SetKebabCaseEndpointNameFormatter();
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.ReceiveEndpoint(_queue, e =>
                            {
                                e.ConfigureConsumer<EmailConsumer>(context);
                            });
                        });
                    });

                    services.AddMassTransitHostedService();
                });
    }
}