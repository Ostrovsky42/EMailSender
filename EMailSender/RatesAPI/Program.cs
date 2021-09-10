using System;
using System.Threading;
using System.Threading.Tasks;
using MailAdmin;
using MassTransit;

namespace RatesAPI
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
                    var subject = await GetValue("subject");
                    var body = await GetValue("body");
                    var mailTo = await GetValue("mailTo");

                    await busControl.Publish<MailAdminExchangeModel>(new
                    {
                        Subject = subject,
                        Body = body,
                        MailTo = mailTo
                    });
                }
            }
            finally
            {
                await busControl.StopAsync();
            }
        }

        private static async Task<string> GetValue(string text)
        {
            text = await Task.Run(() =>
            {
                Console.WriteLine($"Enter {text}");
                Console.Write("> ");
                return Console.ReadLine();
            });

            if ("quit".Equals(text, StringComparison.OrdinalIgnoreCase))
                return text;

            return text;
        }
    }
}