using Agri.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.IO;

namespace SERVERAPI
{
    /// <summary>
    /// The main Program for the application.
    /// </summary>
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    // NLog: setup the logger first to catch all errors
        //    var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        //    try
        //    {
        //        logger.Debug("init main");

        //        var host = BuildWebHost(args);

        //        //RunSeeding(host);

        //        host.Run();
        //    }
        //    catch (Exception ex)
        //    {
        //        //NLog: catch setup errors
        //        logger.Error(ex, "Stopped program because of exception");
        //        throw;
        //    }
        //    finally
        //    {
        //        // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
        //        NLog.LogManager.Shutdown();
        //    }
        //}

        //private static void RunSeeding(IWebHost host)
        //{
        //    //Create a scope outside of the standard web server, which will only exist for the seeding
        //    //of the database, which will only occur when the web server is started.
        //    //This scope won't be affect or affect scopes of the EF activity
        //    var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
        //    using (var scope = scopeFactory.CreateScope())
        //    {
        //        //Get the seeder within this scope
        //        var seeder = scope.ServiceProvider.GetService<AgriSeeder>();
        //        seeder.Seed();
        //    }  //Scope will be closed once seeding is completed
        //}

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            host.Run();
        }

        //public static IWebHost BuildWebHost(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .ConfigureLogging(logging =>
        //        {
        //            logging.ClearProviders();
        //            logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
        //            logging.AddConsole();
        //            logging.AddDebug();
        //        })
        //        //.ConfigureAppConfiguration(SetupConfiguration)
        //        .UseKestrel()
        //        //.UseContentRoot(Directory.GetCurrentDirectory())
        //        .UseIISIntegration()
        //        .UseStartup<Startup>()
        //        .UseNLog()  // NLog: setup NLog for Dependency injection
        //        .UseUrls("http://*:8080")
        //        .Build();

        //private static void SetupConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder builder)
        //{
        //    builder.Sources.Clear();

        //    builder.AddJsonFile("appsettings.json");
        //    builder.AddEnvironmentVariables();
        //}

        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseIISIntegration();
                    //webBuilder.UseKestrel();
                    webBuilder.UseStartup<Startup>();
                });
    }
}