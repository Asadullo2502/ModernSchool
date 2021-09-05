using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Models
{
    public class IndexBallsExcelViewModel
    {
        public int Id { get; set; }
        //public int RegionId { get; set; }
        //public string RegionName { get; set; }
        //public int DistrictId { get; set; }
        //public string DistrictName { get; set; }
        //public int SchoolId { get; set; }
        //public string SchoolName { get; set; }
        public int IndexId { get; set; }
        public string IndexName { get; set; }
        public double? Ball { get; set; }
        public int RootIndex { get; set; }
    }
}
