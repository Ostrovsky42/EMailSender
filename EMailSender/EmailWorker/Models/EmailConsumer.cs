using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EmailWorker.Models
{
    class EmailConsumer: IConsumer<EmailDto>
    {
        ILogger<EmailConsumer> _logger;

        public EmailConsumer(ILogger<EmailConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<EmailDto> context)
        {           
            _logger.LogInformation("Value: {Value}", context.Message.Subject);
        }
    }
}
