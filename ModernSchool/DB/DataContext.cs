using Microsoft.EntityFrameworkCore;
using ModernSchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.DB
{
    public class DataContext : DbContext
    {
        public DbSet<Models.Index> Indexes { get; set; }
        public DbSet<Criteria> Criterias { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<SchoolType> SchoolTypes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<SchoolInfo> SchoolInfos { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<UploadFile> UploadFiles { get; set; }
        public DbSet<IndexBall> IndexBalls { get; set; }
        public DbSet<CurrentYear> CurrentYear { get; set; }





        public DbSet<IndexesDataStatusViewModel> IndexesDataStatuses { get; set; }
        public DbSet<SchoolViewModel> SchoolViewModel { get; set; }
        public DbSet<UserViewModel> UserViewModel { get; set; }
        public DbSet<MaxBallViewModel> MaxBallViewModel { get; set; }





        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    }
}
