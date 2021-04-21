using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Models
{
    public class Department
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string NameUz { get; set; }
        public string NameUzk { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public int? Type { get; set; }

        [ForeignKey(nameof(ParentId))]
        public virtual Department Departments { get; set; }
    }
}
