using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Models
{
    public class PupilInfo
    {
        public int id { get; set; }
        public int? school_id { get; set; }
        public int? kitob_uz_followers { get; set; }
        public int? eye_problem_pupils_count { get; set; }
        public int? speech_problem_pupils_count { get; set; }
        public int? move_problem_pupils_count { get; set; }
        public int? district_level_winners_count { get; set; }
        public int? region_level_winners_count { get; set; }
        public int? republic_level_winners_count { get; set; }
        public int? internation_level_participants_count { get; set; }
        public int? internation_level_prizewinners_count { get; set; }
        public int? internation_level_winners_count { get; set; }
        public int? school_graduates_count { get; set; }
        public int? uzb_enrollees_count { get; set; }
        public int? students_count { get; set; }
        public int? supercontract_students_count { get; set; }
        public double? all_students_scored_ball { get; set; }
        public int? uzb_international_enrollees_count { get; set; }
        public int? top1000_universities_enrolles_count { get; set; }
        public int? other_international_universities_enrollees_count { get; set; }
        public int? middle_spec_univ_pupils_count { get; set; }
        public int? worker_pupils_count { get; set; }
        public DateTime? update_date { get; set; }
        public int? year { get; set; }
        [ForeignKey(nameof(school_id))]
        public virtual School School { get; set; }
        
    }
}
