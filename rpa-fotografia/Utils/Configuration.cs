using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rpa_fotografia.Utils
{
    public static class Configuration
    {
        private static readonly IConfigurationRoot configuration;

        static Configuration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"Configuration/appsettings.json", optional: false, reloadOnChange: true);


            configuration = builder.Build();
        }

        public static string GetSetting(string key)
        {
            return configuration[key];
        }

        public static T GetSection<T>(string sectionName) where T : new()
        {
            var section = new T();
            configuration.GetSection(sectionName).Bind(section);
            return section;
        }
    }
}
