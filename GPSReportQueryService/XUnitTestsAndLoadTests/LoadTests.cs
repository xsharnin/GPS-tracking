using Microsoft.Extensions.Logging;
using Moq;
using QueryServiceEngine;
using QueryServiceEngine.Entities;
using ReceiverEngine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestsAndLoadTests
{
    public class LoadTests
    {
        [Fact]
        public void LoadTestQueryServiceFor1Sec()
        {
            var logger = new Mock<ILogger<IPlotStorageReader>>().Object;
            var receiver = new PlotStorageReader(logger, "localhost");
            var service = new JourneyService(receiver, new PlotsAnalyser(), new Mock<ILogger<JourneyService>>().Object);
            var vehicles = new[]
            {
                "a1","a2", "a3", "a4",
                "b1","b2", "b3", "b4",
                "c1","c2", "c3", "c4",
                "f1","f2", "f3", "f4",
                "d1","d2", "d3", "d4"
            };
            //preWarm
            service.GetAsync("v26", "t0", "t400").Wait();

            var cDict = new ConcurrentDictionary<string, IList<Journey>>();
            var cts = new CancellationTokenSource(1000);
            Parallel.ForEach(
                vehicles,
                new ParallelOptions { CancellationToken = cts.Token, MaxDegreeOfParallelism = 19 },
                async vId =>
                {
                    var step = 0;
                    while (!cts.Token.IsCancellationRequested)
                    {
                        var report = await service.GetAsync(vId, "t50", "t250");
                        cDict.TryAdd(vId + "_" + step, report);
                        step++;
                    }
                });
            Task.Delay(1100).Wait();
            cDict.ToList().ForEach(x => logger.LogTrace(x.Value.Count.ToString()));
            receiver.Dispose();
            throw new Exception($"Query Service did {cDict.Count} reports for 1 sec.");
        }
        [Fact]
        public void LoadTestQueryServiceFor4Sec()
        {
            var logger = new Mock<ILogger<IPlotStorageReader>>().Object;
            var receiver = new PlotStorageReader(logger, "localhost");
            var service = new JourneyService(receiver, new PlotsAnalyser(), new Mock<ILogger<JourneyService>>().Object);
            var vehicles = new[]
              {
              "a1","a2", "a3", "a4",
                "b1","b2", "b3", "b4",
                "c1","c2", "c3", "c4",
                "f1","f2", "f3", "f4",
                "d1","d2", "d3", "d4"
            };
            //preWarm
            var swWarming = Stopwatch.StartNew();
            service.GetAsync("v26", "t0", "t400").Wait();
            swWarming.Stop();

            var cts = new CancellationTokenSource(4000);
            var cLattency = new ConcurrentBag<long>();
            Parallel.ForEach(
                vehicles,
                new ParallelOptions { CancellationToken = cts.Token, MaxDegreeOfParallelism = 19 },
                async vId =>
                {
                    var sw = new Stopwatch();
                    while (!cts.Token.IsCancellationRequested)
                    {
                        sw.Restart();
                        var report = await service.GetAsync(vId, "t0", "t50");
                        sw.Stop();
                        cLattency.Add(sw.ElapsedMilliseconds);
                    }
                });
            Task.Delay(4100).Wait();
            receiver.Dispose();
            throw new Exception(
                $"Query Service did {cLattency.Count} reports for 4 sec. " +
                $"Average processing time for a report = {cLattency.ToList().Sum(x => x) / cLattency.Count} Ms;" +
                $"Warming took {swWarming.ElapsedMilliseconds} ms.");
        }
    }
}
