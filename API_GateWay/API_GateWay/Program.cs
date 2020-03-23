using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.Middleware;
using Ocelot.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Cache.CacheManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

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
                  s.AddHealthChecks();

                  s.AddHealthChecksUI();
                  s.AddOcelot().AddCacheManager(x =>
                  {
                      x.WithDictionaryHandle();
                  });
              })
              .ConfigureLogging((hostingContext, logging) =>
              {
                  logging.AddConsole();
              })
              .Configure(app =>
              {
                  app.UseHealthChecks("/hc", new HealthCheckOptions()
                  {
                      Predicate = _ => true,
                      ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                  });

                  app.UseHealthChecksUI(config => config.UIPath = "/hc-ui");

                  app.UseOcelot().Wait();
              })

              .Build().Run();
        }
    }
}