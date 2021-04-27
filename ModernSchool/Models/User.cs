using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int? RoleId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime? LastSeen { get; set; }
        public int? RegionId { get; set; }
        public int? DistrictId { get; set; }
        public int? SchoolId { get; set; }
        public string Phone { get; set; }

        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; }

        [ForeignKey(nameof(SchoolId))]
        public virtual School School { get; set; }

        [ForeignKey(nameof(DistrictId))]
        public virtual District District { get; set; }

        [ForeignKey(nameof(RegionId))]
        public virtual Region Region { get; set; }


    }
}
