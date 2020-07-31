using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;

namespace RabbitMQClient
{
    public abstract class RabbitConnector
    {
        private ILogger<RabbitConnector> _logger;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;
        protected readonly string _queue;

        public RabbitConnector(ILogger<RabbitConnector> logger, string host, string queue)
        {
            _logger = logger;
            _queue = queue;
            _connection = new ConnectionFactory() { HostName = host }.CreateConnection();
            _channel = _connection.CreateModel();
            try
            {
                var result = _channel.QueueDeclare(queue: queue,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                _logger.LogInformation($"" +
                                  $"QueueName:{result.QueueName} " +
                                  $"ConsumerCount:{result.ConsumerCount} " +
                                  $"MessageCount:{result.MessageCount}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Connaction failed");
            }
        }
    }

}
