using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Models
{
    public class Region
    {
        public int id { get; set; }
        public string name_uz { get; set; }
        public string name_uzk { get; set; }
        public string name_ru { get; set; }
        public string name_en { get; set; }
    }

    public class RegionApiModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string name_ru { get; set; }
    }
}
