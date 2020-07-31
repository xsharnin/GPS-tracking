using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReceiverEngine;
using RabbitMQClient;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Receiver
{
    static class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = new HostBuilder()
                              .ConfigureServices(ConfigurateServices);

            if (Environment.UserInteractive)
            {
                hostBuilder.RunConsoleAsync();
            }
            else
            {
                hostBuilder.Build().RunAsCustomService();
            }               
        }

        private static void ConfigurateServices(HostBuilderContext hostBuilderContext, IServiceCollection services)
        {
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging(ConfigureLoggingBuilder);
            //configure NLog
            ConfigureNlog(services);

            services.AddSingleton<IPlotStorageWriter>(p => new PlotStorageWriter(p.GetRequiredService<ILoggerFactory>().CreateLogger<PlotStorageWriter>(), "localhost", "99"));
            services.AddScoped<ISubscriber>(p => new RabbitMQSubscriber(p.GetRequiredService<ILoggerFactory>().CreateLogger<RabbitMQSubscriber>(), "localhost", "plots_queue"));
            services.AddTransient<IPlotReceiver, PlotReceiver>();
            services.AddScoped<IHostedService, ReceiverHostedService>();
        }
        private static void ConfigureLoggingBuilder(ILoggingBuilder loggingBuilder)
        {
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
