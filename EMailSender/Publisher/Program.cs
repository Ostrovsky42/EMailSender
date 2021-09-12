using System;
using System.Threading;
using System.Threading.Tasks;
using MailTransaction;
using MassTransit;

namespace Publisher
{
    public class Program
    {
        public static async Task Main()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq();

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                while (true)
                {
                    var subject = await getConsoleText("subject");
                    var body = await getConsoleText("body");
                    var displayName = await getConsoleText("displayName");
                    var listMailAddresses = await getConsoleText("listMailAddresses");

                    await busControl.Publish<MailExchangeModel>(new
                    {
                        Subject = subject,
                        Body = body,
                        DisplayName = displayName,
                        ListMailAddresses = listMailAddresses
                    });
                }
            }
            finally
            {
                await busControl.StopAsync();
            }
        }

        private static async Task<string> getConsoleText(string text)
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