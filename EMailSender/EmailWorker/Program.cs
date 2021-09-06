using EmailWorker.Service;
using EmailWorker.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmailWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<EmailConfig>(hostContext.Configuration.GetSection("Email"));

                    services.AddTransient<IEMailSenderService, EMailSenderService>();
                    services.AddHostedService<Worker>();
                });
    }
}
