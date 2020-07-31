using Microsoft.Extensions.Logging;
using Moq;
using ReceiverEngine;
using Xunit;

namespace XUnitTestProject1
{
    public class IntegrationRedisTest
    {
        [Fact]
        public void ShouldReturnFirstPlot()
        {
            var plot = new Plot
            {
                EventCode = EventCode.IgnitionOn,
                Lat = "la0",
                Lon = "lo0",
                TimeStamp = "t0",
                VId = "vtest1"
            };
            plot.InBinary = plot.ToBytes();
            var sender = new PlotStorageWriter(new Mock<ILogger<PlotStorageWriter>>().Object, "localhost", "99");

            Assert.True(sender.AddPlotAsync(plot).Result);
            sender.Dispose();
        }   
    }
}
