using Producer;
using System;
using Xunit;

namespace XUnitTests
{
    public class MessageGeneratorUnitTests
    {
        [Fact]
        public void ShouldReturnFirstPlot()
        {
            var gen = new PlotGenerator();
            var plot = gen.GenerateRandomPlot("v1");

            Assert.Equal("v1", plot.VId);
        }
        [Fact]
        public void ShouldReturnNextPlotAsMovementOrIgnitionOffAfterIgnitionOn()
        {
            var gen = new PlotGenerator();

            var plot = gen.GenerateRandomPlot("v1", new Plot()
            {
                VId = "v1",
                EventCode = EventCode.IgnitionOn
            });
            Assert.Equal("v1", plot.VId);
            Assert.NotEqual(EventCode.IgnitionOn, plot.EventCode);
        }
        [Fact]
        public void ShouldReturnPlotAsIgnitionOnAfterPreviousWasIgnitionOff()
        {
            var gen = new PlotGenerator();

            var plot = gen.GenerateRandomPlot("v1", new Plot()
            {
                VId = "v1",
                EventCode = EventCode.IgnitionOff
            });
            Assert.Equal("v1", plot.VId);
            Assert.Equal(EventCode.IgnitionOn, plot.EventCode);
        }
    }

}
