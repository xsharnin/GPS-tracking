using Producer;
using Xunit;

namespace XUnitTests
{
    public class SenderUnitTests
    {
        [Fact]
        public void ShouldSerializeAndDeserializeMessage()
        {
            var plot = new Plot()
            {
                VId = "v1",
                EventCode = EventCode.IgnitionOff,
                Lat = "la0",
                Lon = "lo0",
                TimeStamp = "t0"
            };
            var bytes = plot.ToBytes();
            var desPlot = bytes.ToPlot();

            Assert.Equal(plot.VId, desPlot.VId);
            Assert.Equal(plot.EventCode, desPlot.EventCode);
            Assert.Equal(plot.Lon, desPlot.Lon);
            Assert.Equal(plot.Lat, desPlot.Lat);
            Assert.Equal(plot.TimeStamp, desPlot.TimeStamp);
        }
    }
}
