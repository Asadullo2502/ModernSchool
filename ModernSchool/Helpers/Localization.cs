using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ModernSchool.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool
{
    public class Localization
    {
        static class ConfigurationManager
        {
            public static IConfiguration AppSetting { get; }
            static ConfigurationManager()
            {
                AppSetting = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();
            }
        }

        public static string GetTranslate(string key, string lang)
        {
            string db_string = ConfigurationManager.AppSetting["ConnectionStrings:DefaultConnection"];
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            var options = optionsBuilder
                    .UseSqlServer(db_string)
                    .Options;
            using DataContext db = new DataContext(options);
            try
            {
                if (lang == "ru")
                {
                    return db.Resources.FirstOrDefault(x => x.Key == key).ValueRu;
                }
                else
                {
                    return db.Resources.FirstOrDefault(x => x.Key == key).ValueUz;
                }
            }
            catch (Exception ex)
            {
                return key;
            }
        }

        
    }
}
