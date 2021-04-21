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
        public int DepartmentId { get; set; }
        public int TypeId { get; set; }
        public int? SubTypeId { get; set; }
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

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }

        [ForeignKey(nameof(TypeId))]
        public virtual SchoolType SchoolType { get; set; }
    }
}
