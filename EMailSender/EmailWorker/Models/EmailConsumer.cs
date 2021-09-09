using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using EventContracts;

namespace EmailWorker.Models
{
    class EmailConsumer : IConsumer<QueueMail>
    {
        ILogger<EmailConsumer> _logger;

        public EmailConsumer(ILogger<EmailConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<QueueMail> context)
        {

            _logger.LogInformation("Value: {Value}", context.Message);

            await Task.CompletedTask;
        }
    }
}
