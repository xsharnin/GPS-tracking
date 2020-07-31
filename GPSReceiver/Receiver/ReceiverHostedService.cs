using Microsoft.Extensions.Hosting;
using RabbitMQClient;
using ReceiverEngine;
using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace Receiver
{
    public class ReceiverHostedService : IHostedService
    {
        private readonly IPlotReceiver _receiver;
        private readonly ISubscriber _subscriber;
        private CancellationToken _cancellationToken;

        public ReceiverHostedService(IPlotReceiver receiver, ISubscriber subscriber)
        {
            _receiver = receiver;
            _subscriber = subscriber;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _cancellationToken.Register(_subscriber.Unsubscribe);
            return Task.Run(()=>_subscriber.Subscribe(_receiver.ReceiveAsync), cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(
                () =>
                {
                    _subscriber.Unsubscribe();
                    NLog.LogManager.Shutdown();
                });
        }           
    }

    public class ReceiverWindowsService : ServiceBase
    {
        private readonly IHost _host;
        private Task _serviceTask;
        private CancellationTokenSource _cancellationTokenSource;

        public ReceiverWindowsService(IHost host)
        {
            _host = host;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        protected override void OnStart(string[] args)
        {
            _serviceTask = _host.StartAsync(_cancellationTokenSource.Token);
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            _cancellationTokenSource.Cancel();
            _host.StopAsync(_cancellationTokenSource.Token);
            _serviceTask.Wait();
            base.OnStop();
        }
    }


    public static class HostedService
    {
        public static void RunAsCustomService(this IHost host)
        {
            var receiverWindowsService = new ReceiverWindowsService(host);
            ServiceBase.Run(receiverWindowsService);
        }
    }
}

