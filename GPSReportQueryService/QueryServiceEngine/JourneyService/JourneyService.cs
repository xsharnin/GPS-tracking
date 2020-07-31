using Microsoft.Extensions.Logging;
using QueryServiceEngine.Entities;
using ReceiverEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace QueryServiceEngine
{
    public class JourneyService : IJourneyService
    {
        private readonly IPlotStorageReader _receiver;
        private readonly IPlotsAnalyser _plotsAnalyser;
        private readonly ILogger<JourneyService> _logger;

        public JourneyService(IPlotStorageReader receiver, IPlotsAnalyser plotsAnalyser, ILogger<JourneyService> logger)
        {
            _receiver = receiver;
            _plotsAnalyser = plotsAnalyser;
            _logger = logger;
        }

        public async Task<List<Journey>> GetAsync(string id, string from, string to)
        {
            var sw = Stopwatch.StartNew();
            var plots = await _receiver.SearchAsync(id, from, to);
            var result = _plotsAnalyser.GetJourneys(plots);
            sw.Stop();
            _logger.LogTrace($"Getting journeys for {id} from {from} to {to} took {sw.ElapsedMilliseconds} ms.");
            return result;
        }
    }
}
