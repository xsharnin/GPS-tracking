using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using WebApiCoreQueryService;

namespace QueryService
{
    static class Program
    {
        static void Main(string[] args)
        {
            var build = CreateWebHostBuilder(args).Build();
            if (Environment.UserInteractive)
            {
                build.Run();
            }
            else
            {
                build.RunAsCustomService();
            };
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
          .UseStartup<Startup>();   
    }
}
