using Microsoft.Extensions.Logging;
using QueryServiceEngine.Entities;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReceiverEngine
{
    public class PlotStorageReader : IPlotStorageReader, IDisposable
    {
        private ILogger<IPlotStorageReader> _logger;
        private readonly IDatabase _cache;
        private ConnectionMultiplexer _connection;

        public PlotStorageReader(ILogger<IPlotStorageReader> logger, string host)
        {
            _connection = ConnectionMultiplexer.Connect(host);
            _cache = _connection.GetDatabase();
            _logger = logger;
        }
        public async Task<IList<Plot>> SearchAsync(string id, string from, string to)
        {
            var plots = new List<Plot>();
            var result = await _cache.SortedSetRangeByScoreAsync(id, from.GetScore(), to.GetScore());
            foreach (var value in result)
            {
                if (value.HasValue)
                {
                    byte[] bytes = value;     
                    plots.Add(bytes.ToPlot());
                }
            }
            _logger.LogDebug($"Received a new message from Redis. Plots count is {plots.Count}");
            return plots;
        }
      
        public void Dispose()
        {
            _connection.Close(true);
        }
    }
}
