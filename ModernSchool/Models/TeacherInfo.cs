using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Models
{
    public class TeacherInfo
    {
        public int id { get; set; }
        public int? school_id { get; set; }
        public int? kitob_uz_followers { get; set; }
        public int? high_edu_teachers_count { get; set; }
        public int? middle_spec_edu_teachers_count { get; set; }
        public int? high_category_teachers_count { get; set; }
        public int? first_category_teachers_count { get; set; }
        public int? second_category_teachers_count { get; set; }
        public int? foreign_langs_teachers_count { get; set; }
        public int? c1_c2_level_teachers_count { get; set; }
        public int? b2_level_teachers_count { get; set; }
        public int? b1_level_teachers_count { get; set; }
        public int? special_teachers_count { get; set; }
        public int? surdo_teachers_count { get; set; }
        public int? tiflo_teachers_count { get; set; }
        public int? olegofreno_teachers_count { get; set; }
        public int? logoped_teachers_count { get; set; }
        public int? other_teachers_count { get; set; }
        public bool? tyutors_existence { get; set; }
        public DateTime? update_date { get; set; }
        public int? year { get; set; }
        [ForeignKey(nameof(school_id))]
        public virtual School School { get; set; }
    }
}
