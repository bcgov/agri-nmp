using Agri.Data;
using AutoMapper;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SERVERAPI;
using System;
using Xunit.Abstractions;

namespace Agri.Tests.Shared
{
    public class TestBase
    {
        private ServiceProvider serviceProvider;
        protected AgriConfigurationContext agriConfigurationDb => serviceProvider.CreateScope().ServiceProvider.GetService<AgriConfigurationContext>();
        protected IMapper Mapper => serviceProvider.CreateScope().ServiceProvider.GetService<IMapper>();

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
                .AddSingleton(configuration)
                .AddHttpContextAccessor();

            foreach (var svc in additionalServices)
            {
                services.AddTransient(svc.svc, svc.impl);
            }

            serviceProvider = services.BuildServiceProvider();
        }

        protected void SeedDatabase()
        {
            if (agriConfigurationDb.Database.IsInMemory())
            {
                var repo = new AgriConfigurationRepository(agriConfigurationDb, Mapper);
                var seeder = new AgriSeeder(agriConfigurationDb, repo);
                seeder.Seed();
            }
        }
    }
}