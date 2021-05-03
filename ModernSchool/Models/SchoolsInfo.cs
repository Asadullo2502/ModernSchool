using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Models
{
    public class SchoolInfo
    {
        public int id { get; set; }
        public int? school_id { get; set; }
        public int? rebuilding_year { get; set; }
        public int? max_pupil_count { get; set; }
        public int? max_teachers_count { get; set; }
        public int? pupils_count { get; set; }
        public int? teachers_count { get; set; }
        public int? microdistrict_pupils_count { get; set; }
        public DateTime? update_date { get; set; }
        public int? year { get; set; }
        [ForeignKey(nameof(school_id))]
        public virtual School School { get; set; }
    }
}
