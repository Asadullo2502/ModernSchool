using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Models
{
    public class Rate
    {
        public int Id { get; set; }
        public int? InspektorId { get; set; }
        public int? SchoolId { get; set; }
        public int? IndexId { get; set; }
        public int? CriteriaId { get; set; }
        public double? ValueSchool { get; set; }
        public double? ValueInspektor { get; set; }
        public int? Year { get; set; }
        public DateTime? UpdateDateSchool { get; set; }
        public DateTime? UpdateDateInspektor { get; set; }

        [ForeignKey(nameof(IndexId))]
        public virtual Index Index { get; set; }

        [ForeignKey(nameof(CriteriaId))]
        public virtual Criteria Criteria { get; set; }
    }
}
