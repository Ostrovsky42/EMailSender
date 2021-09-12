using System.IO;
using EmailWorker.Extentions;
using EmailWorker.Service;
using EmailWorker.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmailWorker
{
    public class Program
    {
        private const string _queueTransaction = "queue-mail";
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
                            cfg.ReceiveEndpoint(_queueTransaction, e =>
                            {
                                e.ConfigureConsumer<MailConsumer>(context);
                            });
                        });
                    });

                    services.AddMassTransitHostedService();
                });
    }
}