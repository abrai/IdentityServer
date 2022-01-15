using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;

namespace IdentityServer
{
    public class Program
    {
        private static string environment;

        public static void Main(string[] args)
        {
            environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Working";
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var defaultConfig = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
           .AddJsonFile(path: string.Format("appsettings.{0}.json", environment), optional: false, reloadOnChange: true)
           .Build();

            var connectionString = defaultConfig.GetValue<string>("ConnectionString");

            var configDictionary = new Dictionary<string, string>
            {
                {"ConnectionString", connectionString},
                {"environment", environment }
            };

            SeedData.InitDB(connectionString);

            //SeedData.EnsureSeedData(connectionString);

            return Host.CreateDefaultBuilder(args)
                 .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                          .AddJsonFile(path: string.Format("appsettings.{0}.json", environment), optional: false, reloadOnChange: true)
                          .AddCommandLine(args)
                          .AddInMemoryCollection(configDictionary);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
