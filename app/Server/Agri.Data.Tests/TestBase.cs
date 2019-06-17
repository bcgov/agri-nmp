using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SERVERAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Xunit.Abstractions;
using AutoMapper;
using Agri.Interfaces;

namespace Agri.Data.Tests
{
    public class TestBase
    {
        private ServiceProvider serviceProvider;
        protected AgriConfigurationContext agriConfigurationDb => serviceProvider.CreateScope().ServiceProvider.GetService<AgriConfigurationContext>();
        protected IMapper Mapper => serviceProvider.CreateScope().ServiceProvider.GetService<IMapper>();
        protected IAgriConfigurationRepository Repo => serviceProvider.CreateScope().ServiceProvider.GetService<IAgriConfigurationRepository>();

        public TestBase(ITestOutputHelper output, params (Type svc, Type impl)[] additionalServices)
        {
            var configuration = A.Fake<IConfiguration>();

            var services = new ServiceCollection()
                .AddLogging(builder => builder.AddProvider(new XUnitLoggerProvider(output)))
                .AddAutoMapper(typeof(Startup))
                .AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<AgriConfigurationContext>(options => options
                    .EnableSensitiveDataLogging()
                    //.EnableDetailedErrors()
                    .UseInMemoryDatabase("Agri_Test")
                    )
                .AddSingleton<IConfiguration>(configuration)
                .AddHttpContextAccessor();

            foreach (var svc in additionalServices)
            {
                services.AddTransient(svc.svc, svc.impl);
            }

            serviceProvider = services.BuildServiceProvider();

            services.AddAutoMapper();

            if (agriConfigurationDb.Database.IsInMemory())
            {
                var seeder = new AgriSeeder(agriConfigurationDb, Repo, Mapper);
                seeder.Seed();
            }
        }
    }
}