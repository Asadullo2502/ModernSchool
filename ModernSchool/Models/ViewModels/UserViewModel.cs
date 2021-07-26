using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime? LastSeen { get; set; }
        public int? RegionId { get; set; }
        public string RegionName { get; set; }
        public int? DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int? SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string Phone { get; set; }

        


    }
}
