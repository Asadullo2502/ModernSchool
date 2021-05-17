using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Models
{
    public class Index
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string ShortName { get; set; }
        public string NameUz { get; set; }
        public string NameUzk { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public double MaxBall { get; set; }
        public int Level { get; set; }
        public int? MenuId { get; set; }

        [ForeignKey(nameof(ParentId))]
        public virtual Index Parent { get; set; }

        public virtual List<Criteria> Criterias { get; set; } 

    }
}
