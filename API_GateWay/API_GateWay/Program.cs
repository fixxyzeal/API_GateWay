using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.Middleware;
using Ocelot.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Cache.CacheManager;

namespace API_GateWay
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new WebHostBuilder()
              .UseKestrel()
              .UseContentRoot(Directory.GetCurrentDirectory())
              .ConfigureAppConfiguration((hostingContext, config) =>
              {
                  config
                      .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                      .AddJsonFile("appsettings.json", true, true)
                      .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                      .AddJsonFile("ocelot.json")
                      .AddEnvironmentVariables();
              })
              .ConfigureServices(s =>
              {
                  s.AddOcelot().AddCacheManager(x =>
                  {
                      x.WithDictionaryHandle();
                  });
              })
              .ConfigureLogging((hostingContext, logging) =>
              {
                  logging.AddConsole();
              })
              .UseIISIntegration()
              .Configure(app =>
              {
                  app.UseOcelot().Wait();
              })

              .Build().Run();
        }
    }
}