using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Models
{
    public class IndexBall
    {
        public int Id { get; set; }
        public int? SchoolId { get; set; }
        public int? IndexId { get; set; }
        public double? SchoolBall { get; set; }
        public double? InspektorBall { get; set; }
        public int? Year { get; set; }
    }
}
