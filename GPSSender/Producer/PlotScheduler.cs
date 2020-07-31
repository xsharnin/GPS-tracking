using Microsoft.Extensions.Logging;
using RabbitMQClient;
using System;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace Producer
{
    public class PlotScheduler : IScheduler
    {
        private readonly IPublisher _publisher;
        private readonly IGenerator _generator;
        private Timer _timer;
        private int i = 0;
        private bool isItStopped = false;
        private string _vehicleId;
        public ILogger<PlotScheduler> _logger;

        //all this parameter we can inject
        //do not do this now because it does not affect performance
        public PlotScheduler(IPublisher publisher, IGenerator generator, ILogger<PlotScheduler> logger)
        {
            _publisher = publisher;
            _generator = generator;
            _logger = logger;
            _timer = new Timer
            {
                AutoReset = true
            };
            _timer.Elapsed += async (source, e) => await SendRandomPlot();

        }

        public void Start(int interval, string vehicleId)
        {
            _timer.Interval = interval;
            _vehicleId = vehicleId;
            _timer.Enabled = true;
        }
        public void Stop()
        {
            _timer.Enabled = false;
            isItStopped = false;            
            while (i > 0)
            {
                _logger.LogInformation($"Stopping Scheduler. Wait {i} plot(s) to be sent");
                Task.Delay(TimeSpan.FromMilliseconds(_timer.Interval)).Wait();
            }
            _vehicleId = string.Empty;
            _logger.LogInformation($"Scheduler stopped.");
        }

        private async Task<bool> SendRandomPlot()
        {
            Interlocked.Increment(ref i);
            var result = await _publisher.PublishAsync(_generator.Generate(_vehicleId));
            Interlocked.Decrement(ref i);
            return result;
        }
    }
}
