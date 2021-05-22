using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Models
{
    public class Criteria
    {
        public int Id { get; set; }
        public int? IndexId { get; set; }
        public string NameUz { get; set; }
        public string NameUzk { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public double MaxBall { get; set; }
        public string Type { get; set; }

        [ForeignKey(nameof(IndexId))]
        public virtual Index Index { get; set; }
    }
}
