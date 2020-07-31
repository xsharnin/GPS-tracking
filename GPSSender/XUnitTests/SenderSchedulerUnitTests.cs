using Microsoft.Extensions.Logging;
using Moq;
using Producer;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using XUnitTests.Stubs;

namespace XUnitTests
{
    public class SenderSchedulerUnitTests
    {
        [Fact]
        public void ShouldSendAMessage()
        {
            var sender = new StubPublisher();
            var scheduler = new PlotScheduler(sender, new PlotGenerator(), new Mock<ILogger<PlotScheduler>>().Object);

            scheduler.Start(100, "v1");
            Task.Delay(150).Wait();
            scheduler.Stop();

            Assert.Single(sender.SentPlots);
        }
        [Theory]
        [InlineData(50, 2, 199)]
        [InlineData(15, 60, 1000)]
        public void ShouldSendMessages(int interval, int messagesNumber, int waitingTime)
        {
            var sender = new StubPublisher();
            var scheduler = new PlotScheduler(sender, new PlotGenerator(), new Mock<ILogger<PlotScheduler>>().Object);

            scheduler.Start(interval, "v1");

            Task.Delay(waitingTime).Wait();
            scheduler.Stop();

            Assert.True(messagesNumber < sender.SentPlots.Count);
            Assert.Equal("v1", sender.SentPlots.FirstOrDefault().VId);
        }
        [Theory]
        [InlineData(500, 2, 1100)]
        [InlineData(400, 2, 900)]
        public void ShouldSendMessagesWithinInterval(int interval, int messagesNumber, int waitingTime)
        {
            var sender = new StubPublisher();
            var scheduler = new PlotScheduler(sender, new PlotGenerator(), new Mock<ILogger<PlotScheduler>>().Object);

            scheduler.Start(interval, "v1");
            Task.Delay(waitingTime).Wait();
            scheduler.Stop();

            var firstPlot = sender.SentPlots.FirstOrDefault();
            var secondPlot = sender.SentPlots.Skip(1).FirstOrDefault();

            Assert.NotEqual(secondPlot.TimeStamp, firstPlot.TimeStamp);
            Assert.Equal("v1", firstPlot.VId);
        }
    }
}

