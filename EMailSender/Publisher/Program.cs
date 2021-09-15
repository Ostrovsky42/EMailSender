using System;
using System.Threading;
using System.Threading.Tasks;
using MailExchange;
using MassTransit;

namespace Publisher
{
    public class Program
    {
        public static async Task Main()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(configure: cfg =>
            {
                // 5672 Основной порт RabbitMQ
                cfg.Host("80.78.240.16", h =>
                {
                    h.Username("nafanya");
                    h.Password("qwe!23");
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                while (true)
                {
                    var subject = await GetConsoleText("subject");
                    var body = await GetConsoleText("body");
                    var displayName = await GetConsoleText("displayName");
                    //var mailAddresses = await GetConsoleText("MailAddresses");

                    await busControl.Publish<IMailExchangeModel>(new
                    {
                        Subject = subject,
                        Body = body,
                        DisplayName = displayName,
                        MailAddresses = "zhekul.90@gmail.com"
                    });
                }
            }
            finally
            {
                await busControl.StopAsync();
            }
        }

        private static async Task<string> GetConsoleText(string text)
        {
            return await Task.Run(() =>
            {
                Console.WriteLine($"Enter { text} (or quit to exit)");
                Console.Write("> ");
                return Console.ReadLine();
            });
        }
    }
}