using System;
using System.Threading;
using System.Threading.Tasks;
using EventContracts;
using MassTransit;

namespace ConsoleEventPublisher
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
                    var amount = await Task.Run(() =>
                    {
                        Console.WriteLine("Enter amount (or quit to exit)");
                        Console.Write("> ");
                        return Console.ReadLine();
                    });

                    if ("quit".Equals(amount, StringComparison.OrdinalIgnoreCase))
                        break;

                    var mailAddresses = await Task.Run(() =>
                    {
                        Console.WriteLine("Enter mailAddresses (or quit to exit)");
                        Console.Write("> ");
                        return Console.ReadLine();
                    });

                    if ("quit".Equals(mailAddresses, StringComparison.OrdinalIgnoreCase))
                        break;

                    await busControl.Publish<QueueMailDeposit>(new
                    {
                        Amount = amount,
                        MailAddresses = mailAddresses
                    });
                }
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}