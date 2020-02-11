using Agri.CalculateService;
using Agri.Data;
using Agri.Models.Settings;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using SERVERAPI.Controllers;
using SERVERAPI.Filters;
using SERVERAPI.Models.Impl;
using SERVERAPI.Utility;
using System;
using System.Globalization;

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
            var agriConnectionString = GetConnectionString();
            //Creates the DbContext as a scoped Service
            services.AddDbContext<AgriConfigurationContext>(options =>
            {
                options.UseNpgsql(agriConnectionString, b => b.MigrationsAssembly("Agri.Data"));
            });
            //services.AddScoped<IConfigurationRepository>(provider => new ConfigurationRepository(agriConnectionString));
            services.AddScoped<SessionTimeoutAttribute>();
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IConfiguration>(Configuration);
            services.Configure<AppSettings>(Configuration);
            services.AddTransient<AgriSeeder>();

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

            //Automapper
            services.AddAutoMapper(typeof(Startup));
            //Mediatr
            services.AddMediatR(typeof(Startup));

            //// Add framework services.
            services.AddMvc()
                .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); })
                .AddJsonOptions(
                    opts =>
                    {
                        opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        opts.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                        opts.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                        opts.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                        // ReferenceLoopHandling is set to Ignore to prevent JSON parser issues with the user / roles model.
                        opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                        opts.SerializerSettings.StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.EscapeNonAscii;
                    });

            services.AddScoped<UserData>();
            services.AddTransient<BrowserData>();
            services.AddScoped<IAgriConfigurationRepository, AgriConfigurationRepository>();
            services.AddTransient<ICalculateAnimalRequirement, CalculateAnimalRequirement>();
            services.AddTransient<ICalculateCropRequirementRemoval, CalculateCropRequirementRemoval>();
            services.AddTransient<ICalculateFertilizerNutrients, CalculateFertilizerNutrients>();
            services.AddTransient<ICalculateManureGeneration, CalculateManureGeneration>();
            services.AddTransient<ICalculateNutrients, CalculateNutrients>();
            services.AddTransient<IChemicalBalanceMessage, ChemicalBalanceMessage>();
            services.AddTransient<IFeedAreaCalculator, FeedAreaCalculator>();
            services.AddTransient<IManureUnitConversionCalculator, ManureUnitConversionCalculator>();
            services.AddTransient<IManureApplicationCalculator, ManureApplicationCalculator>();
            services.AddTransient<IManureLiquidSolidSeparationCalculator, ManureLiquidSolidSeparationCalculator>();
            services.AddTransient<IManureAnimalNumberCalculator, ManureAnimalNumberCalculator>();
            services.AddTransient<IManureOctoberToMarchCalculator, ManureOctoberToMarchCalculator>();
            services.AddTransient<ISoilTestConverter, SoilTestConverter>();
            services.AddTransient<IStorageVolumeCalculator, StorageVolumeCalculator>();

            services.AddOptions();
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

            UpdateDatabase(app);
            RunSeeding(app);
        }

        private string GetConnectionString()
        {
            if (_hostingEnv.IsDevelopment())
            {
                return Configuration["Agri:ConnectionString"];
            }
            else
            {
                var server = Environment.GetEnvironmentVariable("POSTGRESQL_URI");
                var password = Environment.GetEnvironmentVariable("POSTGRESQL_PASSWORD");
                var username = Environment.GetEnvironmentVariable("POSTGRESQL_USER");
                var database = Environment.GetEnvironmentVariable("POSTGRESQL_DATABASE");

                if (string.IsNullOrEmpty(server))
                {
                    throw new Exception(@"Connection String Environment ""POSTGRESQL_URI"" variable not found");
                }
                if (string.IsNullOrEmpty(database))
                {
                    throw new Exception(@"Connection String Environment ""POSTGRESQL_DATABASE"" variable not found");
                }
                if (string.IsNullOrEmpty(username))
                {
                    throw new Exception(@"Connection String Environment ""POSTGRESQL_USER"" variable not found");
                }
                if (string.IsNullOrEmpty(password))
                {
                    throw new Exception(@"Connection String Environment ""POSTGRESQL_PASSWORD"" variable not found");
                }

                return $"Server={server};Database={database};Username={username};Password={password}";
            }
        }

        private void UpdateDatabase(IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetRequiredService<IOptions<AppSettings>>();

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<AgriConfigurationContext>())
                {
                    if (options.Value.RefreshDatabase && _hostingEnv.IsDevelopment())
                    {
                        context.Database.EnsureDeleted();
                    }

                    //If the database is not present or if migrations are required
                    //create the database and/or run the migrations
                    context.Database.Migrate();
                }
            }
        }

        private static void RunSeeding(IApplicationBuilder app)
        {
            //Create a scope outside of the standard web server, which will only exist for the seeding
            //of the database, which will only occur when the web server is started.
            //This scope won't be affect or affect scopes of the EF activity
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //Get the seeder within this scope
                var seeder = serviceScope.ServiceProvider.GetService<AgriSeeder>();
                seeder.Seed();
            }  //Scope will be closed once seeding is completed
        }
    }
}