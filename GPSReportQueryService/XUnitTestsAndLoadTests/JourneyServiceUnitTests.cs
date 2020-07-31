using Microsoft.Extensions.Logging;
using Moq;
using QueryServiceEngine;
using ReceiverEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xunit;

namespace XUnitTestsAndLoadTests
{
    public class JourneyServiceUnitTests
    {
        [Fact]
        public void ShouldReturnJourneys()
        {
            var receiver = new Mock<IPlotStorageReader>();
            receiver.Setup(
                x => x.SearchAsync(
                    It.IsAny<String>(),
                    It.IsAny<String>(),
                    It.IsAny<String>()))
                    .ReturnsAsync(PlotsAnalyserUnitTests.StubPlots);
            var service = new JourneyService(receiver.Object, new PlotsAnalyser());
            var sw = Stopwatch.StartNew();
            var report = service.GetAsync("v1", "t0", "t30").Result;
            sw.Stop();
            var sw2 = Stopwatch.StartNew();
            report = service.GetAsync("v1", "t5", "t500").Result;
            sw2.Stop();
            Assert.Equal(5, report.Count);
        }
    }
}
