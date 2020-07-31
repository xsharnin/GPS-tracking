using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using NLog.Extensions.Logging;
using Producer;
using RabbitMQClient;

namespace Sender
{
    static class Program
    {
        private static int _interval;
        private static string _vehicleId;
        public static void Main(string[] args)
        {
            _vehicleId = args[1];
            _interval = int.Parse(args[3]);
            var hostBuilder = new HostBuilder()
                              .ConfigureServices(ConfigurateServices);

            hostBuilder.RunConsoleAsync();
        }

        private static void ConfigurateServices(HostBuilderContext hostBuilderContext, IServiceCollection services)
        {
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging(ConfigureLoggingBuilder);
            //configure NLog
            ConfigureNlog(services);

            services.AddScoped<IPublisher>(p => new RabbitMQPublisher(p.GetService<ILoggerFactory>().CreateLogger<RabbitMQPublisher>(), "localhost", "plots_queue"));
            services.AddTransient<IGenerator, PlotGenerator>();
            services.AddTransient<IScheduler, PlotScheduler>();
            services.AddScoped<IHostedService>(p => new ProducerHostedService(p.GetService<IScheduler>(), _interval, _vehicleId));

        }
        private static void ConfigureLoggingBuilder(ILoggingBuilder loggingBuilder) {
            loggingBuilder.AddNLog();
            loggingBuilder.SetMinimumLevel(LogLevel.Trace);
        }
        private static void ConfigureNlog(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });

            var config = new NLog.Config.LoggingConfiguration();

            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "log.txt" };
            var tracefile = new NLog.Targets.FileTarget("tracefile") { FileName = "trace.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, logconsole);
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logfile);
            config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, tracefile);

            NLog.LogManager.Configuration = config;
        }
    }
}