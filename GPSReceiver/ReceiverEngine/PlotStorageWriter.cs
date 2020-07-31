using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace ReceiverEngine
{
    public class PlotStorageWriter : IPlotStorageWriter, IDisposable
    {
        private ILogger<PlotStorageWriter> _logger;
        private readonly int _interval;
        private readonly IDatabase _cache;
        private ConnectionMultiplexer _connection;

        public PlotStorageWriter(ILogger<PlotStorageWriter> logger, string host, string interval)
        {
            _connection = ConnectionMultiplexer.Connect(host);
            _cache = _connection.GetDatabase();
            _interval = int.Parse(interval);
            _logger = logger;
        }
        public async Task<bool> AddPlotAsync(Plot plot)
        {
            await _cache.SortedSetAddAsync(plot.VId, plot.InBinary, plot.GetScore(), flags: CommandFlags.FireAndForget);
            _cache.KeyExpire(plot.VId, TimeSpan.FromMinutes(_interval));
            return true;
        }
    
        public void Dispose()
        {
            _connection.Close(true);
        }
    }

}
