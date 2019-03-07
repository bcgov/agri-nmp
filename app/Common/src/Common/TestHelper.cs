using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class TestHelper
    {
        public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets("aspnet-Core2-37757D75-C7BA-4F7A-8B80-0040DB6484EA")
                //.AddEnvironmentVariables()
                .Build();
        }

        public static string GetConnectionString(string outputPath)
        {
            var iConfig = GetIConfigurationRoot(outputPath);

            return iConfig["Agri:ConnectionString"];
        }
    }
}
