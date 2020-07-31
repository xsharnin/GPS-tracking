using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;
using RabbitMQClient;
using Receiver;
using ReceiverEngine;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestProject1
{
    public class LoadTests
    {
        [Fact]
        public void LoadTestRecieverFor1Sec()
        {
            var logger = new Mock<ILogger<RabbitMQSubscriber>>();
            var receiverService = new ReceiverHostedService(
                new PlotReceiver(new PlotStorageWriter(new Mock<ILogger<PlotStorageWriter>>().Object, "localhost", "99")),
                new RabbitMQSubscriber(new Mock<ILogger<RabbitMQSubscriber>>().Object, "localhost", "plots_queue"));
            var cts = new CancellationTokenSource();

            var countBefore = GetRabbitMessageCount();
            receiverService.StartAsync(cts.Token);
            Task.Delay(1000).Wait();
            cts.Cancel();
            receiverService.StopAsync(cts.Token);
            var countAfter = GetRabbitMessageCount();
            var result = countBefore - countAfter;
            throw new Exception($"Receiver did {result} for 1 sec. Before count {countBefore}, after count {countAfter}");
        }
        [Fact]
        public void LoadTestRecieverFor4Sec()
        {
            var logger = new Mock<ILogger<RabbitMQSubscriber>>();
            var receiverService = new ReceiverHostedService(
                new PlotReceiver(new PlotStorageWriter(new Mock<ILogger<PlotStorageWriter>>().Object, "localhost", "99")),
                new RabbitMQSubscriber(new Mock<ILogger<RabbitMQSubscriber>>().Object, "localhost", "plots_queue"));
            var cts = new CancellationTokenSource();

            var countBefore = GetRabbitMessageCount();
            receiverService.StartAsync(cts.Token);
            Task.Delay(4000).Wait();
            cts.Cancel();
            receiverService.StopAsync(cts.Token);
            var countAfter = GetRabbitMessageCount();
            var result = countBefore - countAfter;
            throw new Exception($"Receiver did {result} for 4 sec. Before count {countBefore}, after count {countAfter}");
        }

        private uint GetRabbitMessageCount()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var queue = channel.QueueDeclare(
                    queue: "plots_queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                return queue.MessageCount;
            }
        }
    }
}
