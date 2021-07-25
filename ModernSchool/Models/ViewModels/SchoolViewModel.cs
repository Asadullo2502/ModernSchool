using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Models
{
    public class SchoolViewModel
    {
        public int? Id { get; set; }
        public int? RegionId { get; set; }
        public int? DistrictId { get; set; }
        public int? TypeId { get; set; }
        public int? Number { get; set; }
        public string NameUz { get; set; }
        public string NameUzk { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public string Director { get; set; }
        public string Phone { get; set; }
        public string Latitude { get; set; }
        public string Longtitude { get; set; }
        public string Address { get; set; }
        public int? BuildedYear { get; set; }
        public DateTime? UpdateDate { get; set; }
        public double? ball { get; set; }
        public string RegionName { get; set; }
        public string DistrictName { get; set; }
        public Int64? PageNumber { get; set; }
    }
}
