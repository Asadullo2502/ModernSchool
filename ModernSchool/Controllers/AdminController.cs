using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModernSchool.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModernSchool.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace ModernSchool.Controllers
{
    [Authorize(Roles = "1")]
    public class AdminController : Controller
    {
        public static int _year;

        private DataContext db;
        private DataManager data;
        readonly IWebHostEnvironment _appEnvironment;

        public AdminController(DataContext context, IWebHostEnvironment hostEnvironment)
        {
            db = context;
            data = new DataManager(db);
            _appEnvironment = hostEnvironment;
            _year = db.CurrentYear.First().Year;
        }

        public IActionResult Dashboard()
        {
            return RedirectToAction("Districts");
        }

        #region Departments
        public async Task<IActionResult> Regions()
        {
            return View(await db.Regions.ToListAsync());
        }

        public IActionResult CreateRegion()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateRegion(Region region)
        {
            try
            {
                await db.Regions.AddAsync(region);
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("Regions");
        }

        public async Task<IActionResult> EditRegion(int id = 0)
        {
            return View(await db.Regions.FirstOrDefaultAsync(x => x.id == id));
        }
        
        [HttpPost]
        public async Task<IActionResult> EditRegion(Region region)
        {
            try
            {
                db.Entry(region).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("Regions");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRegion(int id)
        {
            try
            {
                var region = await db.Regions.FirstOrDefaultAsync(x => x.id == id);
                db.Regions.Remove(region);
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("Regions");
        }

        public async Task<IActionResult> Districts(int? RegionId = 0)
        {
            if (RegionId != 0 && RegionId != null)
            {
                var data = await db.Districts.Include(x => x.Region).Where(x => x.parent_id == RegionId).ToListAsync();
                ViewBag.region = RegionId;
                return View(data);
            }
            else
            {
                var data = await db.Districts.Include(x => x.Region).ToListAsync();
                ViewBag.region = 0;
                return View(data);
            }
        }

        public IActionResult CreateDistrict()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateDistrict(District district)
        {
            try
            {
                await db.Districts.AddAsync(district);
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("Districts");
        }

        public async Task<IActionResult> EditDistrict(int id = 0)
        {
            return View(await db.Districts.FirstOrDefaultAsync(x => x.id == id));
        }
        
        [HttpPost]
        public async Task<IActionResult> EditDistrict(District district)
        {
            try
            {
                db.Entry(district).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("Districts");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDistrict(int id)
        {
            try
            {
                var distict = await db.Districts.FirstOrDefaultAsync(x => x.id == id);
                db.Districts.Remove(distict);
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("Districts");
        }
        #endregion

        #region Schools
        public async Task<IActionResult> Schools(int page = 1, int RegionId = 0, int DistrictId = 0)
        {
            string filter = "1=1";
            filter += (RegionId > 0) ? "and r.id = " + RegionId : "";
            filter += (DistrictId > 0) ? "and d.id = " + DistrictId : "";

            var schools = await db.SchoolViewModel.FromSqlRaw(@"
                with db as (
                    select s.*,
                    r.short_name RegionName,
                    d.short_name DistrictName,
                    ROW_NUMBER() OVER(ORDER BY s.RegionId, s.DistrictId, s.Id) AS PageNumber,
                    (
                        select sum(c.MaxBall)
                        from Rates r
                        left join Criterias c on c.Id = r.CriteriaId
                        where r.SchoolId = s.Id
                    ) ball, null ball1,null ball2,null ball3,null ball4
                    from Schools s
                    left join Regions r on r.id = s.RegionId
                    left join Districts d on d.id = s.DistrictId
                    where " + filter + @" 
                )
                select top 10 *
                from db
                WHERE PageNumber > 10 * " + (page - 1) + @"
            ").ToListAsync();

            var ss = await db.Schools.FromSqlRaw(@"
                select s.*,
                ROW_NUMBER() OVER(ORDER BY s.RegionId, s.DistrictId, s.Id) AS PageNumber
                from Schools s
                left join Regions r on r.id = s.RegionId
                left join Districts d on d.id = s.DistrictId
                where " + filter + @" 
            ").ToListAsync();

            int count = ss.Count();
            ViewBag.CurrentPage = page;
            ViewBag.RegionId = RegionId;
            ViewBag.DistrictId = DistrictId;
            ViewBag.PageCount = count / 10 + (count % 10 == 0 ? 0 : 1);

            return View(schools);
        }

        public IActionResult CreateSchool()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateSchool(School school)
        {
            try
            {
                await db.Schools.AddAsync(school);
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("Schools");
        }

        public async Task<IActionResult> EditSchool(int id = 0)
        {
            return View(await db.Schools.Include(x => x.District).FirstOrDefaultAsync(x => x.Id == id));
        }
        
        [HttpPost]
        public async Task<IActionResult> EditSchool(School school)
        {
            try
            {
                db.Entry(school).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("Schools");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSchool(int id)
        {
            try
            {
                var school = await db.Schools.FirstOrDefaultAsync(x => x.Id == id);
                db.Schools.Remove(school);
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("Schools");
        }
        #endregion

        #region Users
        public async Task<IActionResult> Users(int page = 1, int RegionId = 0, int DistrictId = 0)
        {
            string filter = "1=1";
            filter += (RegionId > 0) ? "and r.id = " + RegionId : "";
            filter += (DistrictId > 0) ? "and d.id = " + DistrictId : "";

            var users = await db.UserViewModel.FromSqlRaw(@"
                with db as (
                    select u.*,
                    r.short_name RegionName,
                    d.short_name DistrictName,
                    s.ShortName SchoolName,
                    ro.Name RoleName,
                    ROW_NUMBER() OVER(ORDER BY u.RegionId, u.DistrictId, u.Id) AS PageNumber
                    
                    from Users u
                    left join Regions r on r.id = u.RegionId
                    left join Districts d on d.id = u.DistrictId
                    left join Schools s on s.id = u.SchoolId
                    left join Roles ro on ro.id = u.RoleId
                    where " + filter + @" 
                )
                select top 10 *
                from db
                WHERE PageNumber > 10 * " + (page - 1) + @"
            ").ToListAsync();

            var ss = await db.Users.FromSqlRaw(@"
                select u.*,
                ROW_NUMBER() OVER(ORDER BY u.RegionId, u.DistrictId, u.Id) AS PageNumber
                from Users u
                left join Regions r on r.id = u.RegionId
                left join Districts d on d.id = u.DistrictId
                where " + filter + @" 
            ").ToListAsync();

            int count = ss.Count();
            ViewBag.CurrentPage = page;
            ViewBag.RegionId = RegionId;
            ViewBag.DistrictId = DistrictId;
            ViewBag.PageCount = count / 10 + (count % 10 == 0 ? 0 : 1);

            return View(users);
        }

        public IActionResult CreateUser()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            try
            {
                await db.Users.AddAsync(user);
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("Users");
        }

        public async Task<IActionResult> EditUser(int id = 0)
        {
            return View(await db.Users.FirstOrDefaultAsync(x => x.Id == id));
        }
        
        [HttpPost]
        public async Task<IActionResult> EditUser(User user)
        {
            try
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
                db.Users.Remove(user);
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("Users");
        }
        #endregion

        #region Indexes
        public async Task<IActionResult> Indexes(int? id = 0)
        {
            return View(await db.Indexes.ToListAsync());
        }

        public IActionResult CreateIndex()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateIndex(Models.Index index)
        {
            try
            {
                index.Level = db.Indexes.FirstOrDefault(x => x.Id == index.ParentId).Level + 1;
                await db.Indexes.AddAsync(index);
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("Indexes");
        }

        public async Task<IActionResult> EditIndex(int id = 0)
        {
            return View(await db.Indexes.FirstOrDefaultAsync(x => x.Id == id));
        }
        
        [HttpPost]
        public async Task<IActionResult> EditIndex(Models.Index index)
        {
            try
            {
                index.Level = db.Indexes.FirstOrDefault(x => x.Id == index.ParentId).Level + 1;
                db.Entry(index).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("Indexes");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIndex(int id)
        {
            try
            {
                var index = await db.Indexes.FirstOrDefaultAsync(x => x.Id == id);
                db.Indexes.Remove(index);
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("Indexes");
        }
        #endregion

        #region Criterias
        public async Task<IActionResult> Criterias()
        {
            return View(await db.Criterias.Include(x => x.Index).ToListAsync());
        }

        public IActionResult CreateCriteria()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateCriteria(Criteria criteria)
        {
            try
            {
                await db.Criterias.AddAsync(criteria);
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("Criterias");
        }

        public async Task<IActionResult> EditCriteria(int id = 0)
        {
            return View(await db.Criterias.FirstOrDefaultAsync(x => x.Id == id));
        }
        
        [HttpPost]
        public async Task<IActionResult> EditCriteria(Criteria criteria)
        {
            try
            {
                db.Entry(criteria).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("Criterias");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCriteria(int id)
        {
            try
            {
                var criteria = await db.Criterias.FirstOrDefaultAsync(x => x.Id == id);
                db.Criterias.Remove(criteria);
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("Criterias");
        }
        #endregion

        public async Task<IActionResult> Orders(int page = 1, int RegionId = 0, int DistrictId = 0)
        {

            string filter = "1=1";
            filter += (RegionId > 0) ? "and r.id = " + RegionId : "";
            filter += (DistrictId > 0) ? "and d.id = " + DistrictId : "";

            var schools = await db.SchoolViewModel.FromSqlRaw(@"
                with db as (
                    select s.*,
                    r.short_name RegionName,
                    d.short_name DistrictName,
                    ROUND((
                        select sum(c.MaxBall)
                        from Rates r
                        left join Criterias c on c.Id = r.CriteriaId
                        where  c.Type != 'number' and r.SchoolId = s.Id and r.Year = " + _year + @"
                    ) + 
                    isnull((
                        select sum(i.SchoolBall)
                        from IndexBalls i
                        where i.SchoolId = s.Id and i.Year = " + _year + @"
                    ),0),2) ball,
                    null ball1,
                    null ball2,
                    null ball3,
                    null ball4,
                    ROW_NUMBER() OVER(ORDER BY s.RegionId, s.DistrictId, s.Id) AS PageNumber
                    from Schools s
                    left join Regions r on r.id = s.RegionId
                    left join Districts d on d.id = s.DistrictId
                    where " + filter + @" 
                )
                select top 10 *
                from db
                WHERE PageNumber > 10 * " + (page - 1) + @"
            ").ToListAsync();

            var ss = await db.Schools.FromSqlRaw(@"
                select s.*,
                (
                    select sum(c.MaxBall)
                    from Rates r
                    left join Criterias c on c.Id = r.CriteriaId
                    where r.SchoolId = s.Id
                ) ball,
                ROW_NUMBER() OVER(ORDER BY s.RegionId, s.DistrictId, s.Id) AS PageNumber
                from Schools s
                left join Regions r on r.id = s.RegionId
                left join Districts d on d.id = s.DistrictId
                where " + filter + @" 
            ").ToListAsync();

            int count = ss.Count();
            ViewBag.CurrentPage = page;
            ViewBag.RegionId = RegionId;
            ViewBag.DistrictId = DistrictId;
            ViewBag.PageCount = count / 10 + (count % 10 == 0 ? 0 : 1);
            return View(schools);
        }

        public async Task<IActionResult> RateSchools(int page = 1, int RegionId = 0, int DistrictId = 0)
        {

            string filter = "1=1";
            filter += (RegionId > 0) ? "and r.id = " + RegionId : "";
            filter += (DistrictId > 0) ? "and d.id = " + DistrictId : "";

            var schools = await db.SchoolViewModel.FromSqlRaw(@"
                with db as (
                    select s.*,
                    r.short_name RegionName,
                    d.short_name DistrictName,
                    ROUND((
                        select sum(c.MaxBall)
                        from Rates r
                        left join Criterias c on c.Id = r.CriteriaId
                        where  c.Type != 'number' and r.SchoolId = s.Id and r.Year = " + _year + @"
                    ) + 
                    isnull((
                        select sum(i.SchoolBall)
                        from IndexBalls i
                        where i.SchoolId = s.Id and i.Year = " + _year + @"
                    ),0),2) ball,
                    null ball1,
                    null ball2,
                    null ball3,
                    null ball4,
                    ROW_NUMBER() OVER(ORDER BY s.RegionId, s.DistrictId, s.Id) AS PageNumber
                    from Schools s
                    left join Regions r on r.id = s.RegionId
                    left join Districts d on d.id = s.DistrictId
                    where " + filter + @" 
                )
                select top 10 *
                from db
                WHERE PageNumber > 10 * " + (page - 1) + @"
            ").ToListAsync();

            var ss = await db.Schools.FromSqlRaw(@"
                select s.*,
                (
                        select sum(c.MaxBall)
                        from Rates r
                        left join Criterias c on c.Id = r.CriteriaId
                        where  c.Type != 'number' and r.SchoolId = s.Id and r.Year = " + _year + @"
                    ) + 
                    (
                        select sum(i.SchoolBall)
                        from IndexBalls i
                        where i.SchoolId = s.Id and i.Year = " + _year + @"
                    ) ball,
                ROW_NUMBER() OVER(ORDER BY s.RegionId, s.DistrictId, s.Id) AS PageNumber
                from Schools s
                left join Regions r on r.id = s.RegionId
                left join Districts d on d.id = s.DistrictId
                where " + filter + @" 
            ").ToListAsync();

            int count = ss.Count();
            ViewBag.CurrentPage = page;
            ViewBag.RegionId = RegionId;
            ViewBag.DistrictId = DistrictId;
            ViewBag.PageCount = count / 10 + (count % 10 == 0 ? 0 : 1);
            return View(schools);
        }

        public async Task<IActionResult> RatedSchools(int page = 1, int RegionId = 0, int DistrictId = 0)
        {

            string filter = "1=1";
            filter += (RegionId > 0) ? "and v.Id = " + RegionId : "";
            filter += (DistrictId > 0) ? "and d.Id = " + DistrictId : "";

            var schools = await db.RatedViewModel.FromSqlRaw(@"
                with db as (
                    select 
                        r.SchoolId Id, 
	                    v.name_uz RegionName,
	                    d.name_uz DistrictName, 
	                    s.NameUz, 
                        ROUND((select sum(c.MaxBall)
			                   from Rates r
			                   left join Criterias c on c.Id = r.CriteriaId
			                   left join Indexes i on i.Id = c.IndexId
			                   where c.Type != 'number' and r.SchoolId = s.Id and r.Year = " + _year + @" and r.ValueSchool is not null and i.RootIndex = 1 and i.Id != 96) + 
		                       isnull((select sum(i.SchoolBall)
				               from IndexBalls i
				               left join Indexes ind on ind.Id = i.IndexId
				               where i.SchoolId = s.Id and i.Year = " + _year + @" and i.SchoolBall is not null and ind.RootIndex = 1),0),2) ball1School,
                        ROUND((select sum(c.MaxBall)
			                   from Rates r
			                   left join Criterias c on c.Id = r.CriteriaId
			                   left join Indexes i on i.Id = c.IndexId
			                   where c.Type != 'number' and r.SchoolId = s.Id and r.Year = " + _year + @" and r.ValueSchool is not null and i.RootIndex = 2
		                      ) + isnull((select sum(i.SchoolBall)
				                  from IndexBalls i
				                  left join Indexes ind on ind.Id = i.IndexId
				                  where i.SchoolId = s.Id and i.Year = " + _year + @" and i.SchoolBall is not null and ind.RootIndex = 2),0),2) ball2School,
                        ROUND((select sum(c.MaxBall)
			                   from Rates r
			                   left join Criterias c on c.Id = r.CriteriaId
			                   left join Indexes i on i.Id = c.IndexId
			                   where c.Type != 'number' and r.SchoolId = s.Id and r.Year = " + _year + @" and r.ValueSchool is not null and i.RootIndex = 3
		                       ) + isnull((
				                    select sum(i.SchoolBall)
				                    from IndexBalls i
				                    left join Indexes ind on ind.Id = i.IndexId
				                    where i.SchoolId = s.Id and i.Year = " + _year + @" and i.SchoolBall is not null and ind.RootIndex = 3),0),2) ball3School,
	                    ROUND((select sum(c.MaxBall)
			                   from Rates r
			                   left join Criterias c on c.Id = r.CriteriaId
			                   left join Indexes i on i.Id = c.IndexId
			                   where c.Type != 'number' and r.SchoolId = s.Id and r.Year = " + _year + @" and r.ValueSchool is not null and i.RootIndex = 4
		                       ) + isnull((
				                    select sum(i.SchoolBall)
				                    from IndexBalls i
				                    left join Indexes ind on ind.Id = i.IndexId
				                    where i.SchoolId = s.Id and i.Year = " + _year + @" and i.SchoolBall is not null and ind.RootIndex = 4),0),2) ball4School,
                        ROUND((select sum(c.MaxBall)
			                    from Rates r
			                    left join Criterias c on c.Id = r.CriteriaId
			                    where c.Type != 'number' and r.SchoolId = s.Id and r.Year = " + _year + @" and r.ValueSchool is not null and r.IndexId != 96
		                        ) + isnull((
				                    select sum(i.SchoolBall)
				                    from IndexBalls i
				                    where i.SchoolId = s.Id and i.Year = " + _year + @" and i.SchoolBall is not null),0),2) ballSchool,

                        ROUND((select sum(c.MaxBall)
			                   from Rates r
			                   left join Criterias c on c.Id = r.CriteriaId
			                   left join Indexes i on i.Id = c.IndexId
			                   where c.Type != 'number' and r.SchoolId = s.Id and r.Year = " + _year + @" and r.ValueInspektor is not null and i.RootIndex = 1 and i.Id != 96) + 
		                       isnull((select sum(i.InspektorBall)
				               from IndexBalls i
				               left join Indexes ind on ind.Id = i.IndexId
				               where i.SchoolId = s.Id and i.Year = " + _year + @" and i.InspektorBall is not null and ind.RootIndex = 1),0),2) ball1Inspektor,
                        ROUND((select sum(c.MaxBall)
			                   from Rates r
			                   left join Criterias c on c.Id = r.CriteriaId
			                   left join Indexes i on i.Id = c.IndexId
			                   where c.Type != 'number' and r.SchoolId = s.Id and r.Year = " + _year + @" and r.ValueInspektor is not null and i.RootIndex = 2
		                      ) + isnull((select sum(i.InspektorBall)
				                  from IndexBalls i
				                  left join Indexes ind on ind.Id = i.IndexId
				                  where i.SchoolId = s.Id and i.Year = " + _year + @" and i.InspektorBall is not null and ind.RootIndex = 2),0),2) ball2Inspektor,
                        ROUND((select sum(c.MaxBall)
			                   from Rates r
			                   left join Criterias c on c.Id = r.CriteriaId
			                   left join Indexes i on i.Id = c.IndexId
			                   where c.Type != 'number' and r.SchoolId = s.Id and r.Year = " + _year + @" and r.ValueInspektor is not null and i.RootIndex = 3
		                       ) + isnull((
				                    select sum(i.InspektorBall)
				                    from IndexBalls i
				                    left join Indexes ind on ind.Id = i.IndexId
				                    where i.SchoolId = s.Id and i.Year = " + _year + @" and i.InspektorBall is not null and ind.RootIndex = 3),0),2) ball3Inspektor,
	                    ROUND((select sum(c.MaxBall)
			                   from Rates r
			                   left join Criterias c on c.Id = r.CriteriaId
			                   left join Indexes i on i.Id = c.IndexId
			                   where c.Type != 'number' and r.SchoolId = s.Id and r.Year = " + _year + @" and r.ValueInspektor is not null and i.RootIndex = 4
		                       ) + isnull((
				                    select sum(i.InspektorBall)
				                    from IndexBalls i
				                    left join Indexes ind on ind.Id = i.IndexId
				                    where i.SchoolId = s.Id and i.Year = " + _year + @" and i.InspektorBall is not null and ind.RootIndex = 4),0),2) ball4Inspektor,
                        ROUND((select sum(c.MaxBall)
			                    from Rates r
			                    left join Criterias c on c.Id = r.CriteriaId
			                    where c.Type != 'number' and r.SchoolId = s.Id and r.Year = " + _year + @" and r.ValueInspektor is not null and r.IndexId != 96
		                        ) + isnull((
				                    select sum(i.InspektorBall)
				                    from IndexBalls i
				                    where i.SchoolId = s.Id and i.Year = " + _year + @" and i.InspektorBall is not null),0),2) ballInspektor,

	                    max(r.UpdateDateSchool) UpdateDate,
                        ROW_NUMBER() OVER(ORDER BY max(r.UpdateDateSchool) desc) AS PageNumber
                    from Rates r
                    left join Schools s on s.Id = r.SchoolId
                    left join Districts d on d.id = s.DistrictId
                    left join Regions v on v.Id = s.RegionId
                    where " + filter + @" and r.ValueSchool is not null and (
                        select sum(c.MaxBall)
                        from Rates r
                        left join Criterias c on c.Id = r.CriteriaId
                        where  c.Type != 'number' and r.SchoolId = s.Id and r.Year = " + _year + @"
                    ) + 
                    isnull((
                        select sum(i.SchoolBall)
                        from IndexBalls i
                        where i.SchoolId = s.Id and i.Year = " + _year + @"
                    ),0) > 0
                    group by r.SchoolId, s.NameUz, d.name_uz, v.name_uz, s.Id
                    --order by max(r.UpdateDateSchool) desc
                )
                select top 10 *
                from db
                WHERE PageNumber > 10 * " + (page - 1) + @"
            ").ToListAsync();

            var ss = await db.Schools.FromSqlRaw(@"
                select s.*,
                (
                        select sum(c.MaxBall)
                        from Rates r
                        left join Criterias c on c.Id = r.CriteriaId
                        where  c.Type != 'number' and r.SchoolId = s.Id and r.Year = " + _year + @"
                    ) + 
                    (
                        select sum(i.SchoolBall)
                        from IndexBalls i
                        where i.SchoolId = s.Id and i.Year = " + _year + @"
                    ) ball,
                ROW_NUMBER() OVER(ORDER BY s.RegionId, s.DistrictId, s.Id) AS PageNumber
                from Schools s
                left join Regions v on v.id = s.RegionId
                left join Districts d on d.id = s.DistrictId
                where " + filter + @" and (
                        select sum(c.MaxBall)
                        from Rates r
                        left join Criterias c on c.Id = r.CriteriaId
                        where  c.Type != 'number' and r.SchoolId = s.Id and r.Year = " + _year + @"
                    ) + 
                    isnull((
                        select sum(i.SchoolBall)
                        from IndexBalls i
                        where i.SchoolId = s.Id and i.Year = " + _year + @"
                    ),0) > 0
            ").ToListAsync();

            int count = ss.Count();
            ViewBag.CurrentPage = page;
            ViewBag.RegionId = RegionId;
            ViewBag.DistrictId = DistrictId;
            ViewBag.PageCount = count / 10 + (count % 10 == 0 ? 0 : 1);
            return View(schools.ToList());
        }

        public async Task<IActionResult> CheckIndexes(int? id)
        {
            if (id != null)
            {
                Response.Cookies.Append("school_id", id.ToString());
            }
            
            PageData pageData = new();
            pageData.Rates = await db.Rates.Where(x => x.SchoolId == id && x.Year == _year).ToListAsync();
            pageData.UploadFiles = await db.UploadFiles.Where(x => x.SchoolId == id && x.Year == _year).ToListAsync();
            pageData.Criterias = await db.Criterias.ToListAsync();
            pageData.Indexes = await db.Indexes.Include(x => x.Criterias).ToListAsync();
            pageData.IndexesDataStatuses = await data.IndexesStatusValueInspektor((int)id, _year);
            return View(pageData);
        }

        [HttpPost]
        public async Task<JsonResult> SaveIndex(int indexId, string[] criteriaValues)
        {
            var result = 0;
            int school_id = Convert.ToInt32(Request.Cookies["school_id"]);

            try
            {
                foreach (var item in criteriaValues)
                {
                    var temp = item.Split(';');

                    int CriteriaId = Convert.ToInt32(temp[0]);
                    double ValueInspektor = Convert.ToDouble(temp[1]);
                }

                foreach (var item in criteriaValues)
                {
                    var temp = item.Split(';');

                    int CriteriaId = Convert.ToInt32(temp[0]);
                    double ValueInspektor = Convert.ToDouble(temp[1]);

                    if (CriteriaId == 186 || CriteriaId == 188 || CriteriaId == 190)
                    {
                        List<Rate> _rates = await db.Rates.Where(x => (x.CriteriaId == 186 || x.CriteriaId == 188 || x.CriteriaId == 190) && x.ValueInspektor != null && x.Year == _year && x.SchoolId == school_id).ToListAsync();
                        if (_rates != null)
                        {
                            db.Rates.RemoveRange(_rates);
                            await db.SaveChangesAsync();
                        }
                    }

                    if (CriteriaId == 181 || CriteriaId == 209)
                    {
                        List<Rate> _rates = await db.Rates.Where(x => (x.CriteriaId == 181 || x.CriteriaId == 209) && x.ValueInspektor != null && x.Year == _year && x.SchoolId == school_id).ToListAsync();
                        if (_rates != null)
                        {
                            db.Rates.RemoveRange(_rates);
                            await db.SaveChangesAsync();
                        }
                    }

                    if (CriteriaId == 154 || CriteriaId == 160 || CriteriaId == 167)
                    {
                        List<Rate> _rates = await db.Rates.Where(x => (x.CriteriaId == 154 || x.CriteriaId == 160 || x.CriteriaId == 167) && x.ValueInspektor != null && x.Year == _year && x.SchoolId == school_id).ToListAsync();
                        if (_rates != null)
                        {
                            db.Rates.RemoveRange(_rates);
                            await db.SaveChangesAsync();
                        }
                    }

                    if (CriteriaId == 149 || CriteriaId == 179)
                    {
                        List<Rate> _rates = await db.Rates.Where(x => (x.CriteriaId == 149 || x.CriteriaId == 179) && x.ValueInspektor != null && x.Year == _year && x.SchoolId == school_id).ToListAsync();
                        if (_rates != null)
                        {
                            db.Rates.RemoveRange(_rates);
                            await db.SaveChangesAsync();
                        }
                    }

                    if (CriteriaId == 94 || CriteriaId == 102)
                    {
                        List<Rate> _rates = await db.Rates.Where(x => (x.CriteriaId == 94 || x.CriteriaId == 102) && x.ValueInspektor != null && x.Year == _year && x.SchoolId == school_id).ToListAsync();
                        if (_rates != null)
                        {
                            db.Rates.RemoveRange(_rates);
                            await db.SaveChangesAsync();
                        }
                    }

                    if (CriteriaId == 106 || CriteriaId == 110 || CriteriaId == 148 || CriteriaId == 168 || CriteriaId == 170 || CriteriaId == 177 || CriteriaId == 271)
                    {
                        List<Rate> _rates = await db.Rates.Where(x => (x.CriteriaId == 106 || x.CriteriaId == 110 || x.CriteriaId == 148 || x.CriteriaId == 168 || x.CriteriaId == 170 || x.CriteriaId == 177 || x.CriteriaId == 271) && x.ValueInspektor != null && x.Year == _year && x.SchoolId == school_id).ToListAsync();
                        if (_rates != null)
                        {
                            db.Rates.RemoveRange(_rates);
                            await db.SaveChangesAsync();
                        }
                    }
                }

                List<Rate> rates = await db.Rates.Where(x => x.IndexId == indexId && x.ValueInspektor != null && x.Year == _year && x.SchoolId == school_id).ToListAsync();
                if (rates != null)
                {
                    db.Rates.RemoveRange(rates);
                    await db.SaveChangesAsync();
                }

                foreach (var item in criteriaValues)
                {
                    var temp = item.Split(';');

                    int CriteriaId = Convert.ToInt32(temp[0]);
                    double ValueInspektor = Convert.ToDouble(temp[1]);

                    if (CriteriaId == 186 || CriteriaId == 188 || CriteriaId == 190)
                    {
                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateInspektor = DateTime.Now,
                            IndexId = 140,
                            CriteriaId = 186,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = school_id,
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateInspektor = DateTime.Now,
                            IndexId = 141,
                            CriteriaId = 188,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = school_id,
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateInspektor = DateTime.Now,
                            IndexId = 142,
                            CriteriaId = 190,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = school_id,
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();
                    }

                    else if (CriteriaId == 181 || CriteriaId == 209)
                    {
                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateInspektor = DateTime.Now,
                            IndexId = 101,
                            CriteriaId = 181,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = school_id,
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateInspektor = DateTime.Now,
                            IndexId = 117,
                            CriteriaId = 209,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = school_id,
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();
                    }

                    else if (CriteriaId == 154 || CriteriaId == 160 || CriteriaId == 167)
                    {
                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateInspektor = DateTime.Now,
                            IndexId = 103,
                            CriteriaId = 154,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = school_id,
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateInspektor = DateTime.Now,
                            IndexId = 105,
                            CriteriaId = 160,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = school_id,
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateInspektor = DateTime.Now,
                            IndexId = 107,
                            CriteriaId = 167,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = school_id,
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();
                    }

                    else if (CriteriaId == 149 || CriteriaId == 179)
                    {
                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateInspektor = DateTime.Now,
                            IndexId = 100,
                            CriteriaId = 149,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = school_id,
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateInspektor = DateTime.Now,
                            IndexId = 101,
                            CriteriaId = 179,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = school_id,
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();
                    }

                    else if (CriteriaId == 94 || CriteriaId == 102)
                    {
                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateInspektor = DateTime.Now,
                            IndexId = 86,
                            CriteriaId = 94,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = school_id,
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateInspektor = DateTime.Now,
                            IndexId = 87,
                            CriteriaId = 102,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = school_id,
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();
                    }

                    else if (CriteriaId == 106 || CriteriaId == 110 || CriteriaId == 148 || CriteriaId == 168 || CriteriaId == 170 || CriteriaId == 177 || CriteriaId == 271)
                    {
                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateInspektor = DateTime.Now,
                            IndexId = 88,
                            CriteriaId = 106,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = school_id,
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateInspektor = DateTime.Now,
                            IndexId = 89,
                            CriteriaId = 110,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = school_id,
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateInspektor = DateTime.Now,
                            IndexId = 99,
                            CriteriaId = 148,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = school_id,
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateInspektor = DateTime.Now,
                            IndexId = 107,
                            CriteriaId = 168,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = school_id,
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateInspektor = DateTime.Now,
                            IndexId = 108,
                            CriteriaId = 170,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = school_id,
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateInspektor = DateTime.Now,
                            IndexId = 110,
                            CriteriaId = 177,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = school_id,
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateInspektor = DateTime.Now,
                            IndexId = 94,
                            CriteriaId = 271,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = school_id,
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();
                    }

                    else
                    {
                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateInspektor = DateTime.Now,
                            IndexId = indexId,
                            CriteriaId = Convert.ToInt32(temp[0]),
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = school_id,
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();
                    }
                }
                result = 1;
            }
            catch (Exception e)
            {
                var r = e.Message;
            }

            return Json(result);
        }

        public async Task<IActionResult> OnPostMyUploader(IFormFile MyUploader, int IndexId)
        {
            if (MyUploader != null)
            {
                UploadFile uploadFile = new();
                string uploadsFolder = Path.Combine(_appEnvironment.WebRootPath, "uploads");
                string extension = Path.GetExtension(MyUploader.FileName);
                string fileGuidName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadsFolder, fileGuidName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    MyUploader.CopyTo(fileStream);
                }

                uploadFile.CreateDate = DateTime.Now;
                uploadFile.CreatedBy = 2;
                uploadFile.Year = _year;
                uploadFile.FileExtension = extension;
                uploadFile.FileGuid = fileGuidName;
                uploadFile.FileName = MyUploader.FileName;
                uploadFile.IndexId = IndexId;
                uploadFile.SchoolId = Convert.ToInt32(Request.Cookies["school_id"]);

                await db.UploadFiles.AddAsync(uploadFile);
                await db.SaveChangesAsync();

                return new ObjectResult(new { status = "success" });
            }
            return new ObjectResult(new { status = "fail" });

        }

        [HttpPost]
        public async Task<IActionResult> DeleteUploadFile(int id)
        {
            try
            {
                var item = await db.UploadFiles.FirstOrDefaultAsync(x => x.Id == id);

                FileInfo file = new(_appEnvironment.WebRootPath + "/uploads/" + item.FileGuid);
                if (file.Exists)
                {
                    file.Delete();
                }

                db.UploadFiles.Remove(item);
                await db.SaveChangesAsync();
                return new ObjectResult(new { status = "success" });
            }
            catch { return new ObjectResult(new { status = "fail" }); }

        }

        public async Task<IActionResult> solve()
        {
            var r = await db.Rates.Where(x=>x.ValueSchool != null && x.Year == _year).Select(x=>x.SchoolId).Distinct().ToListAsync();
            foreach (var item in r)
            {
                SolveIndexBallSchool((int)item);
            }
            return RedirectToAction("RatedSchools");
        }
        public async Task<IActionResult> solve2()
        {
            var r = await db.Rates.Where(x => x.ValueSchool != null && x.Year == _year).Select(x => x.SchoolId).Distinct().ToListAsync();
            foreach (var item in r)
            {
                SolveIndexBallInspektor((int)item);
            }
            return RedirectToAction("RatedSchools");
        }

        public int SolveIndexBallSchool(int school_id)
        {
            //int school_id = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value);
            var rates = db.Rates.Where(x => x.SchoolId == school_id && x.ValueSchool != null && x.Year == _year).ToList();

            //Respublika Olimpiada
            try
            {
                double? i = ((10 * rates.FirstOrDefault(x => x.CriteriaId == 79).ValueSchool + 5 * rates.FirstOrDefault(x => x.CriteriaId == 80).ValueSchool + 3 * rates.FirstOrDefault(x => x.CriteriaId == 81).ValueSchool)
                    + (15 * rates.FirstOrDefault(x => x.CriteriaId == 82).ValueSchool + 13 * rates.FirstOrDefault(x => x.CriteriaId == 83).ValueSchool + 10 * rates.FirstOrDefault(x => x.CriteriaId == 84).ValueSchool)
                    + (20 * rates.FirstOrDefault(x => x.CriteriaId == 85).ValueSchool + 18 * rates.FirstOrDefault(x => x.CriteriaId == 86).ValueSchool + 15 * rates.FirstOrDefault(x => x.CriteriaId == 87).ValueSchool))
                    / (rates.FirstOrDefault(x => x.CriteriaId == 106).ValueSchool <= 630 ? 9 : rates.FirstOrDefault(x => x.CriteriaId == 106).ValueSchool <= 945 ? 18 : 27);

                var maxball = data.MaxBallInRepublicOlympiadsSchool(_year);

                double itogBall = 0;
                if (i >= maxball)
                {
                    if (i != 0)
                    {
                        itogBall = 20;
                    }
                    else
                    {
                        itogBall = 0;
                    }
                }
                else
                {
                    itogBall = (double)((20 * i) / maxball);
                }

                TrySaveMaxBall(84, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //Xalqaro Olimpiada
            try
            {
                double? i = 5 * rates.FirstOrDefault(x => x.CriteriaId == 89).ValueSchool + 10 * rates.FirstOrDefault(x => x.CriteriaId == 90).ValueSchool + 20 * rates.FirstOrDefault(x => x.CriteriaId == 91).ValueSchool + 25 * rates.FirstOrDefault(x => x.CriteriaId == 92).ValueSchool;

                var maxball = data.MaxBallInInternationalOlympiadsSchool(_year);

                double itogBall = 0;
                if (i >= maxball)
                {
                    if (i != 0)
                    {
                        itogBall = 25;
                    }
                    else
                    {
                        itogBall = 0;
                    }
                }
                else
                {
                    itogBall = (double)((25 * i) / maxball);
                }

                TrySaveMaxBall(85, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //Oqishga kirish
            try
            {
                double? i = rates.FirstOrDefault(x => x.CriteriaId == 93).ValueSchool / rates.FirstOrDefault(x => x.CriteriaId == 94).ValueSchool;

                var maxball = data.MaxBallInAbiturSchool(_year);

                double itogBall = 0;
                if (i >= maxball)
                {
                    if (i != 0)
                    {
                        itogBall = 25;
                    }
                    else
                    {
                        itogBall = 0;
                    }
                }
                else
                {
                    itogBall = (double)((25 * i) / maxball);
                }

                TrySaveMaxBall(86, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //Bandlik
            try
            {
                double? i = (
                    25 * rates.FirstOrDefault(x => x.CriteriaId == 95).ValueSchool +
                    20 * rates.FirstOrDefault(x => x.CriteriaId == 96).ValueSchool +
                    5 * rates.FirstOrDefault(x => x.CriteriaId == 97).ValueSchool +
                    30 * rates.FirstOrDefault(x => x.CriteriaId == 98).ValueSchool +
                    10 * rates.FirstOrDefault(x => x.CriteriaId == 99).ValueSchool +
                    5 * rates.FirstOrDefault(x => x.CriteriaId == 100).ValueSchool +
                    5 * rates.FirstOrDefault(x => x.CriteriaId == 101).ValueSchool
                    ) / rates.FirstOrDefault(x => x.CriteriaId == 102).ValueSchool;

                var maxball = data.MaxBallInBandlikSchool(_year);

                double itogBall = 0;
                if (i >= maxball)
                {
                    if (i != 0)
                    {
                        itogBall = 100;
                    }
                    else
                    {
                        itogBall = 0;
                    }
                }
                else
                {
                    itogBall = (double)((100 * i) / maxball);
                }

                TrySaveMaxBall(87, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //RespublikaTanlov
            try
            {
                double? i = (
                    5 * rates.FirstOrDefault(x => x.CriteriaId == 103).ValueSchool +
                    10 * rates.FirstOrDefault(x => x.CriteriaId == 104).ValueSchool +
                    15 * rates.FirstOrDefault(x => x.CriteriaId == 105).ValueSchool
                    ) / rates.FirstOrDefault(x => x.CriteriaId == 106).ValueSchool;

                var maxball = data.MaxBallInRespublikaTanlovSchool(_year);

                double itogBall = 0;
                if (i >= maxball)
                {
                    if (i != 0)
                    {
                        itogBall = 15;
                    }
                    else
                    {
                        itogBall = 0;
                    }
                }
                else
                {
                    itogBall = (double)((15 * i) / maxball);
                }

                TrySaveMaxBall(88, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //XalqaroTanlov
            try
            {
                double? i = (
                    10 * rates.FirstOrDefault(x => x.CriteriaId == 107).ValueSchool +
                    20 * rates.FirstOrDefault(x => x.CriteriaId == 108).ValueSchool +
                    25 * rates.FirstOrDefault(x => x.CriteriaId == 109).ValueSchool
                    ) / rates.FirstOrDefault(x => x.CriteriaId == 110).ValueSchool;

                var maxball = data.MaxBallInXalqaroTanlovSchool(_year);

                double itogBall = 0;
                if (i >= maxball)
                {
                    if (i != 0)
                    {
                        itogBall = 25;
                    }
                    else
                    {
                        itogBall = 0;
                    }
                }
                else
                {
                    itogBall = (double)((25 * i) / maxball);
                }

                TrySaveMaxBall(89, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //InklyuzivTalim
            try
            {
                double? i = ((
                    rates.FirstOrDefault(x => x.CriteriaId == 131).ValueSchool +
                    rates.FirstOrDefault(x => x.CriteriaId == 132).ValueSchool +
                    rates.FirstOrDefault(x => x.CriteriaId == 270).ValueSchool
                    ) / rates.FirstOrDefault(x => x.CriteriaId == 271).ValueSchool) * 100;

                double itogBall = 0;
                if (i >= 0.01 && i <= 0.20)
                {
                    itogBall = 1;
                }
                else if (i >= 0.21 && i <= 0.40)
                {
                    itogBall = 2;
                }
                else if (i >= 0.41 && i <= 0.80)
                {
                    itogBall = 3;
                }
                else if (i >= 0.81 && i <= 1.2)
                {
                    itogBall = 4;
                }
                else if (i >= 1.21)
                {
                    itogBall = 5;
                }

                TrySaveMaxBall(94, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //TarbiyaviyIshlar
            try
            {
                double itogBall = 0;
                if (rates.FirstOrDefault(x => x.CriteriaId == 139).ValueSchool == 0 && rates.FirstOrDefault(x => x.CriteriaId == 140).ValueSchool == 0)
                {
                    itogBall = 15;
                }
                else
                {
                    itogBall = -1 * (double)rates.FirstOrDefault(x => x.CriteriaId == 139).ValueSchool + -10 * (double)rates.FirstOrDefault(x => x.CriteriaId == 140).ValueSchool;
                }


                TrySaveMaxBall(96, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //ByudjetdanTashqariMablag
            try
            {
                double bxm = db.BXM.FirstOrDefault(x => x.Year == _year).Price;
                double itogBall = 0;

                double? i = rates.FirstOrDefault(x => x.CriteriaId == 147).ValueSchool / rates.FirstOrDefault(x => x.CriteriaId == 148).ValueSchool;
                if (i < 0.3 * bxm)
                {
                    itogBall = 0;
                }
                else if (i >= 0.3 * bxm && i < 1 * bxm)
                {
                    itogBall = 5;
                }
                else if (i >= 1 * bxm && i < 2 * bxm)
                {
                    itogBall = 10;
                }
                else if (i >= 2 * bxm && i < 3 * bxm)
                {
                    itogBall = 20;
                }
                else if (i >= 3 * bxm)
                {
                    itogBall = 30;
                }


                TrySaveMaxBall(99, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //MikroHudud
            try
            {
                double itogBall = 0;

                double? i = (rates.FirstOrDefault(x => x.CriteriaId == 149).ValueSchool / rates.FirstOrDefault(x => x.CriteriaId == 150).ValueSchool) * 100;

                if (i >= 86)
                {
                    itogBall = 25;
                }
                else if (i >= 76 && i < 86)
                {
                    itogBall = 20;
                }
                else if (i >= 65 && i < 76)
                {
                    itogBall = 15;
                }
                else if (i >= 55 && i < 65)
                {
                    itogBall = 10;
                }
                else if (i >= 51 && i < 55)
                {
                    itogBall = 5;
                }
                else if (i < 51)
                {
                    itogBall = 0;
                }

                TrySaveMaxBall(100, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //KompyuterTaminnot
            try
            {
                double itogBall = 0;

                double? i = 20 * rates.FirstOrDefault(x => x.CriteriaId == 178).ValueSchool / rates.FirstOrDefault(x => x.CriteriaId == 179).ValueSchool +
                            5 * rates.FirstOrDefault(x => x.CriteriaId == 180).ValueSchool / rates.FirstOrDefault(x => x.CriteriaId == 181).ValueSchool +
                            5 * rates.FirstOrDefault(x => x.CriteriaId == 182).ValueSchool / rates.FirstOrDefault(x => x.CriteriaId == 183).ValueSchool;

                var maxball = data.MaxBallInKompTaminnotSchool(_year);
                if (i >= maxball)
                {
                    if (i != 0)
                    {
                        itogBall = 30;
                    }
                    else
                    {
                        itogBall = 0;
                    }
                }
                else
                {
                    itogBall = (double)((30 * i) / maxball);
                }

                TrySaveMaxBall(101, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //OliyMalumot
            try
            {
                double itogBall = 0;

                double? i = (rates.FirstOrDefault(x => x.CriteriaId == 153).ValueSchool / rates.FirstOrDefault(x => x.CriteriaId == 154).ValueSchool) * 100;

                if (i >= 97)
                {
                    itogBall = 20;
                }
                else if (i >= 91 && i < 97)
                {
                    itogBall = 10;
                }
                else if (i >= 86 && i < 91)
                {
                    itogBall = 5;
                }
                else if (i < 86)
                {
                    itogBall = 0;
                }

                TrySaveMaxBall(103, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //Toifa
            try
            {
                double itogBall = 0;

                double? i = ((rates.FirstOrDefault(x => x.CriteriaId == 158).ValueSchool + rates.FirstOrDefault(x => x.CriteriaId == 159).ValueSchool) / rates.FirstOrDefault(x => x.CriteriaId == 160).ValueSchool) * 100;

                if (i >= 25)
                {
                    itogBall = 100;
                }
                else if (i >= 24 && i < 25)
                {
                    itogBall = 80;
                }
                else if (i >= 23 && i < 24)
                {
                    itogBall = 75;
                }
                else if (i >= 22 && i < 23)
                {
                    itogBall = 70;
                }
                else if (i >= 21 && i < 22)
                {
                    itogBall = 65;
                }
                else if (i >= 20 && i < 21)
                {
                    itogBall = 60;
                }
                else if (i >= 19 && i < 20)
                {
                    itogBall = 55;
                }
                else if (i >= 18 && i < 19)
                {
                    itogBall = 50;
                }
                else if (i >= 17 && i < 18)
                {
                    itogBall = 45;
                }
                else if (i >= 16 && i < 17)
                {
                    itogBall = 40;
                }
                else if (i >= 15 && i < 16)
                {
                    itogBall = 35;
                }
                else if (i >= 14 && i < 15)
                {
                    itogBall = 30;
                }
                else if (i >= 13 && i < 14)
                {
                    itogBall = 25;
                }
                else if (i >= 12 && i < 13)
                {
                    itogBall = 20;
                }
                else if (i >= 11 && i < 12)
                {
                    itogBall = 15;
                }
                else if (i >= 10 && i < 11)
                {
                    itogBall = 10;
                }
                else if (i >= 9 && i < 10)
                {
                    itogBall = 5;
                }
                else if (i < 9)
                {
                    itogBall = 0;
                }

                TrySaveMaxBall(105, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //ChetTili
            try
            {
                double itogBall = 0;

                double? i = (
                                30 * rates.FirstOrDefault(x => x.CriteriaId == 161).ValueSchool +
                                20 * rates.FirstOrDefault(x => x.CriteriaId == 162).ValueSchool +
                                10 * rates.FirstOrDefault(x => x.CriteriaId == 163).ValueSchool
                             ) /
                             rates.FirstOrDefault(x => x.CriteriaId == 164).ValueSchool;

                var maxball = data.MaxBallInChetTiliSchool(_year);
                if (i >= maxball)
                {
                    if (i != 0)
                    {
                        itogBall = 30;
                    }
                    else
                    {
                        itogBall = 0;
                    }
                }
                else
                {
                    itogBall = (double)((30 * i) / maxball);
                }

                TrySaveMaxBall(106, school_id, itogBall);

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //Kitob.uz
            try
            {
                double itogBall = 0;

                double? i = ((rates.FirstOrDefault(x => x.CriteriaId == 165).ValueSchool + rates.FirstOrDefault(x => x.CriteriaId == 166).ValueSchool) / (rates.FirstOrDefault(x => x.CriteriaId == 167).ValueSchool + rates.FirstOrDefault(x => x.CriteriaId == 168).ValueSchool)) * 100;

                if (i >= 86)
                {
                    itogBall = 10;
                }
                else if (i >= 61 && i < 86)
                {
                    itogBall = 8;
                }
                else if (i >= 31 && i < 61)
                {
                    itogBall = 4;
                }
                else if (i >= 11 && i < 31)
                {
                    itogBall = 2;
                }
                else if (i < 11)
                {
                    itogBall = 0;
                }

                TrySaveMaxBall(107, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //Kitobxonlik
            try
            {
                double itogBall = 0;

                double? i = rates.FirstOrDefault(x => x.CriteriaId == 169).ValueSchool / rates.FirstOrDefault(x => x.CriteriaId == 170).ValueSchool;

                if (i >= 10)
                {
                    itogBall = 10;
                }
                else if (i >= 6 && i < 10)
                {
                    itogBall = 8;
                }
                else if (i >= 4 && i < 6)
                {
                    itogBall = 4;
                }
                else if (i >= 2 && i < 4)
                {
                    itogBall = 2;
                }
                else if (i < 2)
                {
                    itogBall = 0;
                }

                TrySaveMaxBall(108, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //KitobTaminot
            try
            {
                double itogBall = 0;

                double? i = rates.FirstOrDefault(x => x.CriteriaId == 176).ValueSchool / rates.FirstOrDefault(x => x.CriteriaId == 177).ValueSchool;

                if (i >= 5)
                {
                    itogBall = 10;
                }
                else if (i >= 4 && i < 5)
                {
                    itogBall = 8;
                }
                else if (i >= 3 && i < 4)
                {
                    itogBall = 4;
                }
                else if (i >= 2 && i < 3)
                {
                    itogBall = 2;
                }
                else if (i < 2)
                {
                    itogBall = 0;
                }

                TrySaveMaxBall(110, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //MebelJihoz
            try
            {
                double itogBall = 0;

                double? i = (rates.FirstOrDefault(x => x.CriteriaId == 208).ValueSchool / rates.FirstOrDefault(x => x.CriteriaId == 209).ValueSchool) * 100;

                if (i >= 100)
                {
                    itogBall = 30;
                }
                else if (i >= 81 && i < 100)
                {
                    itogBall = 25;
                }
                else if (i >= 51 && i < 81)
                {
                    itogBall = 20;
                }
                else if (i >= 21 && i < 51)
                {
                    itogBall = 10;
                }
                else if (i >= 11 && i < 21)
                {
                    itogBall = 5;
                }
                else if (i < 11)
                {
                    itogBall = 0;
                }

                TrySaveMaxBall(117, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //LokalTarmoq
            try
            {
                double itogBall = 0;

                itogBall = (double)(10 * (rates.FirstOrDefault(x => x.CriteriaId == 185).ValueSchool / rates.FirstOrDefault(x => x.CriteriaId == 186).ValueSchool));

                TrySaveMaxBall(140, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //InternetTarmoq
            try
            {
                double itogBall = 0;

                itogBall = (double)(10 * (rates.FirstOrDefault(x => x.CriteriaId == 187).ValueSchool / rates.FirstOrDefault(x => x.CriteriaId == 188).ValueSchool));

                TrySaveMaxBall(141, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //KompYengiligi
            try
            {
                double itogBall = 0;

                itogBall = (double)(5 * (rates.FirstOrDefault(x => x.CriteriaId == 189).ValueSchool / rates.FirstOrDefault(x => x.CriteriaId == 190).ValueSchool));

                TrySaveMaxBall(142, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //BirMillionDasturchi
            try
            {
                double itogBall = 0;

                double? i = (rates.FirstOrDefault(x => x.CriteriaId == 283).ValueSchool / rates.FirstOrDefault(x => x.CriteriaId == 284).ValueSchool) * 100;

                if (i >= 10)
                {
                    itogBall = 10;
                }
                else if (i >= 5 && i < 10)
                {
                    itogBall = 8;
                }
                else if (i >= 1 && i < 5)
                {
                    itogBall = 5;
                }
                else if (i < 1)
                {
                    itogBall = 0;
                }

                TrySaveMaxBall(148, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return 1;
        }

        public int SolveIndexBallInspektor(int school_id)
        {
            //int school_id = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value);
            var rates = db.Rates.Where(x => x.SchoolId == school_id && x.ValueInspektor != null && x.Year == _year).ToList();

            //Respublika Olimpiada
            try
            {
                double? i = ((10 * rates.FirstOrDefault(x => x.CriteriaId == 79).ValueInspektor + 5 * rates.FirstOrDefault(x => x.CriteriaId == 80).ValueInspektor + 3 * rates.FirstOrDefault(x => x.CriteriaId == 81).ValueInspektor)
                    + (15 * rates.FirstOrDefault(x => x.CriteriaId == 82).ValueInspektor + 13 * rates.FirstOrDefault(x => x.CriteriaId == 83).ValueInspektor + 10 * rates.FirstOrDefault(x => x.CriteriaId == 84).ValueInspektor)
                    + (20 * rates.FirstOrDefault(x => x.CriteriaId == 85).ValueInspektor + 18 * rates.FirstOrDefault(x => x.CriteriaId == 86).ValueInspektor + 15 * rates.FirstOrDefault(x => x.CriteriaId == 87).ValueInspektor))
                    / (rates.FirstOrDefault(x => x.CriteriaId == 106).ValueInspektor <= 630 ? 9 : rates.FirstOrDefault(x => x.CriteriaId == 106).ValueInspektor <= 945 ? 18 : 27);

                var maxball = data.MaxBallInRepublicOlympiadsInspektor(_year);

                double itogBall = 0;
                if (i >= maxball)
                {
                    if (i != 0)
                    {
                        itogBall = 20;
                    }
                    else
                    {
                        itogBall = 0;
                    }
                }
                else
                {
                    itogBall = (double)((20 * i) / maxball);
                }

                TrySaveMaxBallInspektor(84, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //Xalqaro Olimpiada
            try
            {
                double? i = 5 * rates.FirstOrDefault(x => x.CriteriaId == 89).ValueInspektor + 10 * rates.FirstOrDefault(x => x.CriteriaId == 90).ValueInspektor + 20 * rates.FirstOrDefault(x => x.CriteriaId == 91).ValueInspektor + 25 * rates.FirstOrDefault(x => x.CriteriaId == 92).ValueInspektor;

                var maxball = data.MaxBallInInternationalOlympiadsInspektor(_year);

                double itogBall = 0;
                if (i >= maxball)
                {
                    if (i != 0)
                    {
                        itogBall = 25;
                    }
                    else
                    {
                        itogBall = 0;
                    }
                }
                else
                {
                    itogBall = (double)((25 * i) / maxball);
                }

                TrySaveMaxBallInspektor(85, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //Oqishga kirish
            try
            {
                double? i = rates.FirstOrDefault(x => x.CriteriaId == 93).ValueInspektor / rates.FirstOrDefault(x => x.CriteriaId == 94).ValueInspektor;

                var maxball = data.MaxBallInAbiturInspektor(_year);

                double itogBall = 0;
                if (i >= maxball)
                {
                    if (i != 0)
                    {
                        itogBall = 25;
                    }
                    else
                    {
                        itogBall = 0;
                    }
                }
                else
                {
                    itogBall = (double)((25 * i) / maxball);
                }

                TrySaveMaxBallInspektor(86, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //Bandlik
            try
            {
                double? i = (
                    25 * rates.FirstOrDefault(x => x.CriteriaId == 95).ValueInspektor +
                    20 * rates.FirstOrDefault(x => x.CriteriaId == 96).ValueInspektor +
                    5 * rates.FirstOrDefault(x => x.CriteriaId == 97).ValueInspektor +
                    30 * rates.FirstOrDefault(x => x.CriteriaId == 98).ValueInspektor +
                    10 * rates.FirstOrDefault(x => x.CriteriaId == 99).ValueInspektor +
                    5 * rates.FirstOrDefault(x => x.CriteriaId == 100).ValueInspektor +
                    5 * rates.FirstOrDefault(x => x.CriteriaId == 101).ValueInspektor
                    ) / rates.FirstOrDefault(x => x.CriteriaId == 102).ValueInspektor;

                var maxball = data.MaxBallInBandlikInspektor(_year);

                double itogBall = 0;
                if (i >= maxball)
                {
                    if (i != 0)
                    {
                        itogBall = 100;
                    }
                    else
                    {
                        itogBall = 0;
                    }
                }
                else
                {
                    itogBall = (double)((100 * i) / maxball);
                }

                TrySaveMaxBallInspektor(87, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //RespublikaTanlov
            try
            {
                double? i = (
                    5 * rates.FirstOrDefault(x => x.CriteriaId == 103).ValueInspektor +
                    10 * rates.FirstOrDefault(x => x.CriteriaId == 104).ValueInspektor +
                    15 * rates.FirstOrDefault(x => x.CriteriaId == 105).ValueInspektor
                    ) / rates.FirstOrDefault(x => x.CriteriaId == 106).ValueInspektor;

                var maxball = data.MaxBallInRespublikaTanlovInspektor(_year);

                double itogBall = 0;
                if (i >= maxball)
                {
                    if (i != 0)
                    {
                        itogBall = 15;
                    }
                    else
                    {
                        itogBall = 0;
                    }
                }
                else
                {
                    itogBall = (double)((15 * i) / maxball);
                }

                TrySaveMaxBallInspektor(88, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //XalqaroTanlov
            try
            {
                double? i = (
                    10 * rates.FirstOrDefault(x => x.CriteriaId == 107).ValueInspektor +
                    20 * rates.FirstOrDefault(x => x.CriteriaId == 108).ValueInspektor +
                    25 * rates.FirstOrDefault(x => x.CriteriaId == 109).ValueInspektor
                    ) / rates.FirstOrDefault(x => x.CriteriaId == 110).ValueInspektor;

                var maxball = data.MaxBallInXalqaroTanlovInspektor(_year);

                double itogBall = 0;
                if (i >= maxball)
                {
                    if (i != 0)
                    {
                        itogBall = 25;
                    }
                    else
                    {
                        itogBall = 0;
                    }
                }
                else
                {
                    itogBall = (double)((25 * i) / maxball);
                }

                TrySaveMaxBallInspektor(89, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //InklyuzivTalim
            try
            {
                double? i = ((
                    rates.FirstOrDefault(x => x.CriteriaId == 131).ValueInspektor +
                    rates.FirstOrDefault(x => x.CriteriaId == 132).ValueInspektor +
                    rates.FirstOrDefault(x => x.CriteriaId == 270).ValueInspektor
                    ) / rates.FirstOrDefault(x => x.CriteriaId == 271).ValueInspektor) * 100;

                double itogBall = 0;
                if (i >= 0.01 && i <= 0.20)
                {
                    itogBall = 1;
                }
                else if (i >= 0.21 && i <= 0.40)
                {
                    itogBall = 2;
                }
                else if (i >= 0.41 && i <= 0.80)
                {
                    itogBall = 3;
                }
                else if (i >= 0.81 && i <= 1.2)
                {
                    itogBall = 4;
                }
                else if (i >= 1.21)
                {
                    itogBall = 5;
                }

                TrySaveMaxBallInspektor(94, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //TarbiyaviyIshlar
            try
            {
                double itogBall = 0;
                if (rates.FirstOrDefault(x => x.CriteriaId == 139).ValueInspektor == 0 && rates.FirstOrDefault(x => x.CriteriaId == 140).ValueInspektor == 0)
                {
                    itogBall = 15;
                }
                else
                {
                    itogBall = -1 * (double)rates.FirstOrDefault(x => x.CriteriaId == 139).ValueInspektor + -10 * (double)rates.FirstOrDefault(x => x.CriteriaId == 140).ValueInspektor;
                }


                TrySaveMaxBallInspektor(96, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //ByudjetdanTashqariMablag
            try
            {
                double bxm = db.BXM.FirstOrDefault(x => x.Year == _year).Price;
                double itogBall = 0;

                double? i = rates.FirstOrDefault(x => x.CriteriaId == 147).ValueInspektor / rates.FirstOrDefault(x => x.CriteriaId == 148).ValueInspektor;
                if (i < 0.3 * bxm)
                {
                    itogBall = 0;
                }
                else if (i >= 0.3 * bxm && i < 1 * bxm)
                {
                    itogBall = 5;
                }
                else if (i >= 1 * bxm && i < 2 * bxm)
                {
                    itogBall = 10;
                }
                else if (i >= 2 * bxm && i < 3 * bxm)
                {
                    itogBall = 20;
                }
                else if (i >= 3 * bxm)
                {
                    itogBall = 30;
                }


                TrySaveMaxBallInspektor(99, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //MikroHudud
            try
            {
                double itogBall = 0;

                double? i = (rates.FirstOrDefault(x => x.CriteriaId == 149).ValueInspektor / rates.FirstOrDefault(x => x.CriteriaId == 150).ValueInspektor) * 100;

                if (i >= 86)
                {
                    itogBall = 25;
                }
                else if (i >= 76 && i < 86)
                {
                    itogBall = 20;
                }
                else if (i >= 65 && i < 76)
                {
                    itogBall = 15;
                }
                else if (i >= 55 && i < 65)
                {
                    itogBall = 10;
                }
                else if (i >= 51 && i < 55)
                {
                    itogBall = 5;
                }
                else if (i < 51)
                {
                    itogBall = 0;
                }

                TrySaveMaxBallInspektor(100, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //KompyuterTaminnot
            try
            {
                double itogBall = 0;

                double? i = 20 * rates.FirstOrDefault(x => x.CriteriaId == 178).ValueInspektor / rates.FirstOrDefault(x => x.CriteriaId == 179).ValueInspektor +
                            5 * rates.FirstOrDefault(x => x.CriteriaId == 180).ValueInspektor / rates.FirstOrDefault(x => x.CriteriaId == 181).ValueInspektor +
                            5 * rates.FirstOrDefault(x => x.CriteriaId == 182).ValueInspektor / rates.FirstOrDefault(x => x.CriteriaId == 183).ValueInspektor;

                var maxball = data.MaxBallInKompTaminnotInspektor(_year);
                if (i >= maxball)
                {
                    if (i != 0)
                    {
                        itogBall = 30;
                    }
                    else
                    {
                        itogBall = 0;
                    }
                }
                else
                {
                    itogBall = (double)((30 * i) / maxball);
                }

                TrySaveMaxBallInspektor(101, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //OliyMalumot
            try
            {
                double itogBall = 0;

                double? i = (rates.FirstOrDefault(x => x.CriteriaId == 153).ValueInspektor / rates.FirstOrDefault(x => x.CriteriaId == 154).ValueInspektor) * 100;

                if (i >= 97)
                {
                    itogBall = 20;
                }
                else if (i >= 91 && i < 97)
                {
                    itogBall = 10;
                }
                else if (i >= 86 && i < 91)
                {
                    itogBall = 5;
                }
                else if (i < 86)
                {
                    itogBall = 0;
                }

                TrySaveMaxBallInspektor(103, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //Toifa
            try
            {
                double itogBall = 0;

                double? i = ((rates.FirstOrDefault(x => x.CriteriaId == 158).ValueInspektor + rates.FirstOrDefault(x => x.CriteriaId == 159).ValueInspektor) / rates.FirstOrDefault(x => x.CriteriaId == 160).ValueInspektor) * 100;

                if (i >= 25)
                {
                    itogBall = 100;
                }
                else if (i >= 24 && i < 25)
                {
                    itogBall = 80;
                }
                else if (i >= 23 && i < 24)
                {
                    itogBall = 75;
                }
                else if (i >= 22 && i < 23)
                {
                    itogBall = 70;
                }
                else if (i >= 21 && i < 22)
                {
                    itogBall = 65;
                }
                else if (i >= 20 && i < 21)
                {
                    itogBall = 60;
                }
                else if (i >= 19 && i < 20)
                {
                    itogBall = 55;
                }
                else if (i >= 18 && i < 19)
                {
                    itogBall = 50;
                }
                else if (i >= 17 && i < 18)
                {
                    itogBall = 45;
                }
                else if (i >= 16 && i < 17)
                {
                    itogBall = 40;
                }
                else if (i >= 15 && i < 16)
                {
                    itogBall = 35;
                }
                else if (i >= 14 && i < 15)
                {
                    itogBall = 30;
                }
                else if (i >= 13 && i < 14)
                {
                    itogBall = 25;
                }
                else if (i >= 12 && i < 13)
                {
                    itogBall = 20;
                }
                else if (i >= 11 && i < 12)
                {
                    itogBall = 15;
                }
                else if (i >= 10 && i < 11)
                {
                    itogBall = 10;
                }
                else if (i >= 9 && i < 10)
                {
                    itogBall = 5;
                }
                else if (i < 9)
                {
                    itogBall = 0;
                }

                TrySaveMaxBallInspektor(105, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //ChetTili
            try
            {
                double itogBall = 0;

                double? i = (
                                30 * rates.FirstOrDefault(x => x.CriteriaId == 161).ValueInspektor +
                                20 * rates.FirstOrDefault(x => x.CriteriaId == 162).ValueInspektor +
                                10 * rates.FirstOrDefault(x => x.CriteriaId == 163).ValueInspektor
                             ) /
                             rates.FirstOrDefault(x => x.CriteriaId == 164).ValueInspektor;

                var maxball = data.MaxBallInChetTiliInspektor(_year);
                if (i >= maxball)
                {
                    if (i != 0)
                    {
                        itogBall = 30;
                    }
                    else
                    {
                        itogBall = 0;
                    }
                }
                else
                {
                    itogBall = (double)((30 * i) / maxball);
                }

                TrySaveMaxBallInspektor(106, school_id, itogBall);

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //Kitob.uz
            try
            {
                double itogBall = 0;

                double? i = ((rates.FirstOrDefault(x => x.CriteriaId == 165).ValueInspektor + rates.FirstOrDefault(x => x.CriteriaId == 166).ValueInspektor) / (rates.FirstOrDefault(x => x.CriteriaId == 167).ValueInspektor + rates.FirstOrDefault(x => x.CriteriaId == 168).ValueInspektor)) * 100;

                if (i >= 86)
                {
                    itogBall = 10;
                }
                else if (i >= 61 && i < 86)
                {
                    itogBall = 8;
                }
                else if (i >= 31 && i < 61)
                {
                    itogBall = 4;
                }
                else if (i >= 11 && i < 31)
                {
                    itogBall = 2;
                }
                else if (i < 11)
                {
                    itogBall = 0;
                }

                TrySaveMaxBallInspektor(107, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //Kitobxonlik
            try
            {
                double itogBall = 0;

                double? i = rates.FirstOrDefault(x => x.CriteriaId == 169).ValueInspektor / rates.FirstOrDefault(x => x.CriteriaId == 170).ValueInspektor;

                if (i >= 10)
                {
                    itogBall = 10;
                }
                else if (i >= 6 && i < 10)
                {
                    itogBall = 8;
                }
                else if (i >= 4 && i < 6)
                {
                    itogBall = 4;
                }
                else if (i >= 2 && i < 4)
                {
                    itogBall = 2;
                }
                else if (i < 2)
                {
                    itogBall = 0;
                }

                TrySaveMaxBallInspektor(108, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //KitobTaminot
            try
            {
                double itogBall = 0;

                double? i = rates.FirstOrDefault(x => x.CriteriaId == 176).ValueInspektor / rates.FirstOrDefault(x => x.CriteriaId == 177).ValueInspektor;

                if (i >= 5)
                {
                    itogBall = 10;
                }
                else if (i >= 4 && i < 5)
                {
                    itogBall = 8;
                }
                else if (i >= 3 && i < 4)
                {
                    itogBall = 4;
                }
                else if (i >= 2 && i < 3)
                {
                    itogBall = 2;
                }
                else if (i < 2)
                {
                    itogBall = 0;
                }

                TrySaveMaxBallInspektor(110, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //MebelJihoz
            try
            {
                double itogBall = 0;

                double? i = (rates.FirstOrDefault(x => x.CriteriaId == 208).ValueInspektor / rates.FirstOrDefault(x => x.CriteriaId == 209).ValueInspektor) * 100;

                if (i >= 100)
                {
                    itogBall = 30;
                }
                else if (i >= 81 && i < 100)
                {
                    itogBall = 25;
                }
                else if (i >= 51 && i < 81)
                {
                    itogBall = 20;
                }
                else if (i >= 21 && i < 51)
                {
                    itogBall = 10;
                }
                else if (i >= 11 && i < 21)
                {
                    itogBall = 5;
                }
                else if (i < 11)
                {
                    itogBall = 0;
                }

                TrySaveMaxBallInspektor(117, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //LokalTarmoq
            try
            {
                double itogBall = 0;

                itogBall = (double)(10 * (rates.FirstOrDefault(x => x.CriteriaId == 185).ValueInspektor / rates.FirstOrDefault(x => x.CriteriaId == 186).ValueInspektor));

                TrySaveMaxBallInspektor(140, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //InternetTarmoq
            try
            {
                double itogBall = 0;

                itogBall = (double)(10 * (rates.FirstOrDefault(x => x.CriteriaId == 187).ValueInspektor / rates.FirstOrDefault(x => x.CriteriaId == 188).ValueInspektor));

                TrySaveMaxBallInspektor(141, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //KompYengiligi
            try
            {
                double itogBall = 0;

                itogBall = (double)(5 * (rates.FirstOrDefault(x => x.CriteriaId == 189).ValueInspektor / rates.FirstOrDefault(x => x.CriteriaId == 190).ValueInspektor));

                TrySaveMaxBallInspektor(142, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //BirMillionDasturchi
            try
            {
                double itogBall = 0;

                double? i = (rates.FirstOrDefault(x => x.CriteriaId == 283).ValueInspektor / rates.FirstOrDefault(x => x.CriteriaId == 284).ValueInspektor) * 100;

                if (i >= 10)
                {
                    itogBall = 10;
                }
                else if (i >= 5 && i < 10)
                {
                    itogBall = 8;
                }
                else if (i >= 1 && i < 5)
                {
                    itogBall = 5;
                }
                else if (i < 1)
                {
                    itogBall = 0;
                }

                TrySaveMaxBallInspektor(148, school_id, itogBall);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return 1;
        }

        ////////////////////////////////////////////////////////////////////////////
        public void TrySaveMaxBall(int indexId, int schoolId, double itogBall)
        {
            try
            {
                var res = db.IndexBalls.FirstOrDefault(x => x.IndexId == indexId && x.SchoolId == schoolId && x.Year == _year);
                if (res != null)
                {
                    if (res.SchoolBall != itogBall)
                    {
                        res.SchoolBall = itogBall;
                        db.Entry(res).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                {
                    db.IndexBalls.Add(new IndexBall
                    {
                        IndexId = indexId,
                        SchoolId = schoolId,
                        Year = _year,
                        SchoolBall = itogBall
                    });
                    db.SaveChanges();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public void TrySaveMaxBallInspektor(int indexId, int schoolId, double itogBall)
        {
            try
            {
                var res = db.IndexBalls.FirstOrDefault(x => x.IndexId == indexId && x.SchoolId == schoolId && x.Year == _year);
                if (res != null)
                {
                    if (res.InspektorBall != itogBall)
                    {
                        res.InspektorBall = itogBall;
                        db.Entry(res).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                {
                    db.IndexBalls.Add(new IndexBall
                    {
                        IndexId = indexId,
                        SchoolId = schoolId,
                        Year = _year,
                        InspektorBall = itogBall
                    });
                    db.SaveChanges();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
