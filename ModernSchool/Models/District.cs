using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Models
{
    public class District
    {
        public int id { get; set; }
        public int parent_id { get; set; }
        public string name_uz { get; set; }
        public string name_uzk { get; set; }
        public string name_ru { get; set; }
        public string name_en { get; set; }
        [ForeignKey(nameof(parent_id))]
        public virtual Region Region { get; set; }
    }

    public class DistrictApiModel
    {
        public int district_id { get; set; }
        public int city_id { get; set; }
        public string district_name { get; set; }
        public string district_name_ru { get; set; }
    }
}
