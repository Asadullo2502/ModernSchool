using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Models
{
    public class SchoolMenu
    {
        public int id { get; set; }
        public int menu_id { get; set; }
        public int criteria_id { get; set; }
        public int order_by { get; set; }
        [ForeignKey(nameof(menu_id))]
        public virtual Menu Menu { get; set; }
        [ForeignKey(nameof(criteria_id))]
        public virtual Criteria Criteria { get; set; }

    }
}
