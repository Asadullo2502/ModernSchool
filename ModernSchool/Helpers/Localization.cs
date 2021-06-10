using Microsoft.EntityFrameworkCore;
using ModernSchool.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool
{
    public class Localization
    {
        public static string GetTranslate(string key, string lang)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            var options = optionsBuilder
                    .UseSqlServer(@"data source=83.69.136.11,10002;initial catalog=ModernSchool;persist security info=True;user id=sa;password=web@1234;MultipleActiveResultSets=true")
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
