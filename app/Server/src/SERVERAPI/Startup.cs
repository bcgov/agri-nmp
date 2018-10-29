/*
 
 *
 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.XPath;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;

using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.FileProviders;
using SERVERAPI.Models;
using SERVERAPI.Utility;
using SERVERAPI.Controllers;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Agri.Data;

namespace SERVERAPI
{
    /// <summary>
    /// The application Startup class
    /// </summary>
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnv;

        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            _hostingEnv = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddAuthorization();
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IConfiguration>(Configuration);
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));


            //// allow for large files to be uploaded
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 1073741824; // 1 GB
            });
            services.AddResponseCompression();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = ".NMP.Session";
                options.IdleTimeout = TimeSpan.FromHours(4);
            });

            // Enable Node Services
            services.AddNodeServices();

            var agriConnectionString = Configuration["Agri:ConnectionString"];
            Console.WriteLine(agriConnectionString);
            //services.AddDbContext<AgriConfigurationContext>(options =>
            //    { options.UseNpgsql(Configuration.GetConnectionString("AgriConnectionString")
            //                                    ,b => b.MigrationsAssembly("SERVERAPI"));
            //    });
            services.AddDbContext<AgriConfigurationContext>(options =>
                {
                    options.UseNpgsql(agriConnectionString, b => b.MigrationsAssembly("SERVERAPI"));
                });

            //// Add framework services.
            services.AddMvc()
                .AddJsonOptions(
                    opts =>
                    {
                        opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        opts.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                        opts.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                        opts.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                        // ReferenceLoopHandling is set to Ignore to prevent JSON parser issues with the user / roles model.
                        opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });

            services.AddScoped<SERVERAPI.Models.Impl.UserData>();
            services.AddScoped<SERVERAPI.Models.Impl.StaticData>();
            services.AddScoped<SERVERAPI.Models.Impl.BrowserData>();
            services.AddOptions();
            //services.AddScoped<SERVERAPI.Utility.CalculateNutrients>();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            var cultureInfo = new CultureInfo("en-US");
            cultureInfo.NumberFormat.CurrencySymbol = "$";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseSession();
            app.UseResponseCompression();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }    
}
