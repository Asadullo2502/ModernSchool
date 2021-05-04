using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Models
{
    public class InternationOlympiadWinner
    {
        public int id { get; set; }
        public int? school_id { get; set; }
        public int? subject_id { get; set; }
        public int? participants { get; set; }
        public int? bronze { get; set; }
        public int? silver { get; set; }
        public int? gold { get; set; }
        public DateTime? update_date { get; set; }
        public int? year { get; set; }
        [ForeignKey(nameof(school_id))]
        public virtual School School { get; set; }
        [ForeignKey(nameof(subject_id))]
        public virtual Subject Subject { get; set; }
    }
}
