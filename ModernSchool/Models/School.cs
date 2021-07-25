using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Models
{
    public class School
    {
        public int Id { get; set; }
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

        [ForeignKey(nameof(RegionId))]
        public virtual Region Region { get; set; }
        [ForeignKey(nameof(DistrictId))]
        public virtual District District { get; set; }

        [ForeignKey(nameof(TypeId))]
        public virtual SchoolType SchoolType { get; set; }
        public virtual SchoolInfo SchoolInfo { get; set; }
        //public virtual TeacherInfo TeacherInfo { get; set; }
        //public virtual PupilInfo PupilInfo { get; set; }
        //public virtual List<RepublicOlympiadWinner> RepublicOlympiadWinners { get; set; }
        //public virtual List<InternationOlympiadWinner> InternationOlympiadWinners { get; set; }
        //[NotMapped]
        //public List<Subject> Subjects { get; set; }
        [NotMapped]
        public double ball { get; set; }
        [NotMapped]
        public string RegionName { get; set; }
        [NotMapped]
        public string DistrictName { get; set; }
    }

    public class SchoolApiModel
    {
        public int id { get; set; }
        public int district_id { get; set; }
        public string name { get; set; }
        public string name_ru { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string langs { get; set; }
        public string school_address_type { get; set; }
        public string distanse_to_city { get; set; }
        public int? max_pupils { get; set; }
        public int? now_pupils { get; set; }
        public double? coefficient { get; set; }
        public int? available_classes { get; set; }
        public int? teachers_count { get; set; }
        public int? highest_category_teachers_count { get; set; }
        public int? first_category_teachers_count { get; set; }
    }
}
