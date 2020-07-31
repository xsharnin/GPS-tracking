using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading.Tasks;

namespace RabbitMQClient
{
    public class RabbitMQSubscriber : RabbitConnector, ISubscriber, IDisposable
    {
        private string _consumerTag;
        private ILogger<RabbitMQSubscriber> _logger;

        public RabbitMQSubscriber(ILogger<RabbitMQSubscriber> logger, string host, string queue) : base(logger, host, queue)
        {
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            _logger = logger;
        }
        public void Subscribe(Func<byte[], Task> receiverCommand)
        {
            try
            {
                var eventingBasicConsumer = new EventingBasicConsumer(_channel);
                eventingBasicConsumer.Received += async (model, ea)  =>
                {
                    _logger.LogInformation($" [x] Received message with {ea.Body.Length} length");
                    await receiverCommand(ea.Body);
                };
                _consumerTag = _channel.BasicConsume(
                    queue: _queue,
                    autoAck: true,
                    consumer: eventingBasicConsumer);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Subscription failed");
            }
        }

        public void Unsubscribe()
        {
            _channel.BasicCancel(_consumerTag);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }

}
