using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Models
{
    public class RepublicOlympiadWinner
    {
        public int id { get; set; }
        public int? school_id { get; set; }
        public int? subject_id { get; set; }
        public int? participants { get; set; }
        public int? district_first_place { get; set; }
        public int? district_second_place { get; set; }
        public int? district_third_place { get; set; }
        public int? region_first_place { get; set; }
        public int? region_second_place { get; set; }
        public int? region_third_place { get; set; }
        public int? republic_first_place { get; set; }
        public int? republic_second_place { get; set; }
        public int? republic_third_place { get; set; }
        public DateTime? update_date { get; set; }
        public int? year { get; set; }
        [ForeignKey(nameof(school_id))]
        public virtual School School { get; set; }
        [ForeignKey(nameof(subject_id))]
        public virtual Subject Subject { get; set; }
    }
}
