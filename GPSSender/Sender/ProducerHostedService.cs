using Microsoft.Extensions.Hosting;
using Producer;
using System.Threading;
using System.Threading.Tasks;

namespace Sender
{
    public class ProducerHostedService : IHostedService
    {
        private readonly IScheduler _scheduler;
        private readonly int _interval;
        private readonly string _vehicleId;
        private CancellationToken _cancellationToken;

        public ProducerHostedService(IScheduler scheduler, int interval, string vehicleId)
        {
            _scheduler = scheduler;
            _interval = interval;
            _vehicleId = vehicleId;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _cancellationToken.Register(_scheduler.Stop);
            return Task.Run(() => _scheduler.Start(_interval, _vehicleId), cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(
                () =>
                {
                    _scheduler.Stop();
                    NLog.LogManager.Shutdown();
                });
        }
    }
}
