using Producer;
using RabbitMQClient;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace XUnitTests.Stubs
{
    public class StubPublisher : IPublisher
    {
        public readonly ConcurrentQueue<Plot> SentPlots;
        public StubPublisher()
        {
            SentPlots = new ConcurrentQueue<Plot>();
        }
        public Task<bool> PublishAsync(byte[] plot)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    SentPlots.Enqueue(plot.ToPlot());
                    return true;
                });
        }
    }
}
