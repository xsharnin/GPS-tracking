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
    public class IntegrationJourneysServiceTest
    {
        [Fact]
        public void ShouldReturnJourneys()
        {
            var logger = new Mock<ILogger<IPlotStorageReader>>().Object;
            var sender = new PlotStorageReader(logger, "localhost");
            var service = new JourneyService(sender, new PlotsAnalyser());
            var sw = Stopwatch.StartNew();
            var report = service.GetAsync("v1", "t0", "t30").Result;
            sw.Stop();
            var sw2 = Stopwatch.StartNew();
            report = service.GetAsync("v2", "t5", "t500").Result;
            sw2.Stop();
            Assert.False(report.Count==0);
            sender.Dispose();
        }
    }
}
