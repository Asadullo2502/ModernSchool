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
        public DbSet<TeacherInfo> TeacherInfos { get; set; }
        public DbSet<PupilInfo> PupilInfos { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<RepublicOlympiadWinner> RepublicOlympiadWinners { get; set; }
        public DbSet<InternationOlympiadWinner> InternationOlympiadWinners { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<SchoolMenu> SchoolMenus { get; set; }
        public DbSet<Resource> Resources { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    }
}
