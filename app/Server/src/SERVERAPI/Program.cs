using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Agri.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace SERVERAPI
{
    /// <summary>
    /// The main Program for the application.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            //var config = new ConfigurationBuilder()
            //    .AddJsonFile("config.json", false, true)
            //    .AddEnvironmentVariables("")
            //    .Build();

            //var url = config["ASPNETCORE_URLS"] ?? "http://*:8080";

            //var host = new WebHostBuilder()
            //    .UseKestrel()
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseIISIntegration()
            //    .UseStartup<Startup>()
            //    .UseUrls(url)
            //    .Build();  

            //host.Run();
            var host = BuildWebHost(args);

            RunSeeding(host);

            host.Run();
        }

        private static void RunSeeding(IWebHost host)
        {
            //Create a scope outside of the standard web server, which will only exist for the seeding
            //of the database, which will only occur when the web server is started.
            //This scope won't be affect or affect scopes of the EF activity
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                //Get the seeder within this scope
                var seeder = scope.ServiceProvider.GetService<AgriSeeder>();
                seeder.Seed();
            }  //Scope will be closed once seeding is completed
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(SetupConfiguration)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls("http://*:8080")
                .Build();


        private static void SetupConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder builder)
        {
            builder.Sources.Clear();

            builder.AddEnvironmentVariables();
        }
    }

}
