using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;

namespace RabbitMQClient
{
    public class RabbitMQPublisher : RabbitConnector, IPublisher, IDisposable
    {
        private readonly IBasicProperties _properties;
        private ILogger<RabbitMQPublisher> _logger;

        public RabbitMQPublisher(ILogger<RabbitMQPublisher> logger, string host, string queue) : base(logger, host, queue)
        {
            _properties = _channel.CreateBasicProperties();
            _properties.Persistent = true;
            _logger = logger;
        }
        public Task<bool> PublishAsync(byte[] plot)
        {
            _logger.LogTrace($"New message is sent at {DateTime.Now:O}");
            return Task.Factory.StartNew(() => Publish(plot));
        }

        private bool Publish(byte[] plot)
        {
            try
            {
                _channel.BasicPublish(
                    exchange: "",
                    routingKey: _queue,
                    basicProperties: _properties,
                    body: plot);
                _logger.LogTrace($" [x] Sent {plot.Length}");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Publish failed");
            }
            return false;
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }

}
