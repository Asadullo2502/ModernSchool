using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Models
{
    public class SchoolType
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public bool IsSpec { get; set; }
        public string NameUz { get; set; }
        public string NameUzk { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
    }
}
