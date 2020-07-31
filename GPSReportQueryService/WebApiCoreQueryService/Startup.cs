using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using QueryServiceEngine;
using ReceiverEngine;

namespace WebApiCoreQueryService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;  
            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging(ConfigureLoggingBuilder);

            services.AddSingleton<IPlotStorageReader>(p => new PlotStorageReader(p.GetService<ILoggerFactory>().CreateLogger<PlotStorageReader>(), "localhost"));
            services.AddTransient<IPlotsAnalyser, PlotsAnalyser>();
            services.AddTransient<IJourneyService, JourneyService>();
            services.AddMvc();

            var serviceProvider = services.BuildServiceProvider();

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            //configure NLog
            ConfigureNlog(loggerFactory);
        }

        private static void ConfigureNlog(ILoggerFactory loggerFactory)
        {
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

        private static void ConfigureLoggingBuilder(ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.AddNLog();
            loggingBuilder.SetMinimumLevel(LogLevel.Trace);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
