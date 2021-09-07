using EmailWorker.Service;
using EmailWorker.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RatesApi.Extensions;
using System.IO;

namespace EmailWorker
{
    public class Program
    {
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
                    services.Configure<EmailConfig>(hostContext.Configuration.GetSection("Gmail"));
                    services.AddOptions<EmailConfig>().Bind(configuration.GetSection(nameof(EmailConfig)));
                    services.AddTransient<IEMailSenderService, EMailSenderService>();
                    services.AddHostedService<Worker>();
                });
    }
}