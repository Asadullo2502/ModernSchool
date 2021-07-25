﻿using Microsoft.AspNetCore.Mvc;
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
        private DataContext db;
        private DataManager data;
        readonly IWebHostEnvironment _appEnvironment;

        public AdminController(DataContext context, IWebHostEnvironment hostEnvironment)
        {
            db = context;
            data = new DataManager(db);
            _appEnvironment = hostEnvironment;
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
        public async Task<IActionResult> Schools()
        {
            return View(await db.Schools.Include(x => x.District).Include(x => x.Region).ToListAsync());
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
        public async Task<IActionResult> Users()
        {
            return View(await db.Users.Include(x => x.Role).Include(x => x.Region).Include(x => x.District).Include(x => x.School).ToListAsync());
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
            //if (id != null && id != 0)
            //{
            //    return View(await db.Indexes.Include(x => x.Parent).Where(x => x.ParentId == id).ToListAsync());
            //}
            {
                return View(await db.Indexes.ToListAsync());
            }
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
        

        public async Task<IActionResult> CheckIndexes(int? id)
        {
            if (id != null)
            {
                Response.Cookies.Append("school_id", id.ToString());
            }
            
            PageData pageData = new();
            pageData.Rates = await db.Rates.Where(x => x.SchoolId == id).ToListAsync();
            pageData.UploadFiles = await db.UploadFiles.Where(x => x.SchoolId == id).ToListAsync();
            pageData.Criterias = await db.Criterias.ToListAsync();
            pageData.Indexes = await db.Indexes.Include(x => x.Criterias).ToListAsync();
            pageData.IndexesDataStatuses = await data.IndexesStatus((int)id);
            return View(pageData);
        }

        [HttpPost]
        public async Task<JsonResult> SaveIndex(int indexId, string[] criteriaValues)
        {
            var result = 0;

            try
            {
                List<Rate> rates = await db.Rates.Where(x => x.IndexId == indexId && x.ValueInspektor != null).ToListAsync();
                if (rates != null)
                {
                    db.Rates.RemoveRange(rates);
                    await db.SaveChangesAsync();
                }
                foreach (var item in criteriaValues)
                {
                    var temp = item.Split(';');

                    var rate = new Rate
                    {
                        UpdateDateSchool = DateTime.Now,
                        IndexId = indexId,
                        CriteriaId = Convert.ToInt32(temp[0]),
                        ValueInspektor = Convert.ToDouble(temp[1]),
                        SchoolId = Convert.ToInt32(Request.Cookies["school_id"]),
                        Year = 2021,

                    };

                    await db.Rates.AddAsync(rate);
                    await db.SaveChangesAsync();
                }
                result = 1;
            }
            catch (Exception e)
            {
                var r = e.Message;
            }

            return Json(result);
        }

        public IActionResult OnPostMyUploader(IFormFile MyUploader, int IndexId)
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
                uploadFile.FileExtension = extension;
                uploadFile.FileGuid = fileGuidName;
                uploadFile.FileName = MyUploader.FileName;
                uploadFile.IndexId = IndexId;
                uploadFile.SchoolId = Convert.ToInt32(Request.Cookies["school_id"]);

                db.UploadFiles.Add(uploadFile);
                db.SaveChanges();

                return new ObjectResult(new { status = "success" });
            }
            return new ObjectResult(new { status = "fail" });

        }

        [HttpPost]
        public IActionResult DeleteUploadFile(int id)
        {
            try
            {
                var item = db.UploadFiles.FirstOrDefault(x => x.Id == id);

                FileInfo file = new(_appEnvironment.WebRootPath + "/uploads/" + item.FileGuid);
                if (file.Exists)
                {
                    file.Delete();
                }

                db.UploadFiles.Remove(item);
                db.SaveChanges();
                return new ObjectResult(new { status = "success" });
            }
            catch { return new ObjectResult(new { status = "fail" }); }

        }

        //public async Task<IActionResult> SchoolProfile(int? id)
        //{
        //    if (id != null)
        //    {
        //        Response.Cookies.Append("school_id", id.ToString());
        //        School school = await db.Schools.Include(x => x.Region).Include(x => x.District).FirstOrDefaultAsync(x => x.Id == (int)id);
        //        return View(school);
        //    }
        //    else
        //    {
        //        int school_id = Convert.ToInt32(Request.Cookies["school_id"]);
        //        School school = await db.Schools.Include(x => x.Region).Include(x => x.District).FirstOrDefaultAsync(x => x.Id == school_id);
        //        return View(school);
        //    }
        //}
        //public async Task<IActionResult> Questionnaire(int menu_id)
        //{
        //    int school_id = Convert.ToInt32(Request.Cookies["school_id"]);
        //    PageData pageData = new()
        //    {
        //        Rates = await db.Rates.Where(x => x.SchoolId == school_id).ToListAsync(),
        //        Criterias = await db.Criterias.ToListAsync(),
        //        SchoolMenus = await db.SchoolMenus.Include(x => x.Criteria.Index).Include(x => x.Menu).Where(x => x.menu_id == menu_id).ToListAsync()
        //    };
        //    return View(pageData);
        //}
        //public async Task<IActionResult> MainInfo()
        //{
        //    int school_id = Convert.ToInt32(Request.Cookies["school_id"]);
        //    return View(await db.Schools.Include(x => x.Region).Include(x => x.District).Include(x => x.SchoolType).Include(x => x.SchoolInfo).FirstOrDefaultAsync(x => x.Id == school_id));
        //}
        //public async Task<IActionResult> TeachersInfo()
        //{
        //    int school_id = Convert.ToInt32(Request.Cookies["school_id"]);
        //    return View(await db.Schools.Include(x => x.SchoolInfo).Include(x => x.TeacherInfo).FirstOrDefaultAsync(x => x.Id == school_id));
        //}
        //public async Task<IActionResult> PupilsInfo()
        //{
        //    int school_id = Convert.ToInt32(Request.Cookies["school_id"]);
        //    var model = await db.Schools.Include(x => x.SchoolInfo).Include(x => x.PupilInfo).Include(x => x.InternationOlympiadWinners).Include(x => x.RepublicOlympiadWinners).FirstOrDefaultAsync(x => x.Id == school_id);
        //    model.Subjects = await db.Subjects.ToListAsync();
        //    return View(model);
        //}
        //#region Menu
        //public async Task<IActionResult> Menu()
        //{
        //    return View(await db.Menus.ToListAsync());
        //}

        //public IActionResult CreateMenu()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> CreateMenu(Menu menu)
        //{
        //    try
        //    {
        //        await db.Menus.AddAsync(menu);
        //        await db.SaveChangesAsync();
        //    }
        //    catch { }
        //    return RedirectToAction("Menu");
        //}

        //public async Task<IActionResult> EditMenu(int id = 0)
        //{
        //    return View(await db.Menus.FirstOrDefaultAsync(x => x.id == id));
        //}
        //[HttpPost]
        //public async Task<IActionResult> EditMenu(Menu menu)
        //{
        //    try
        //    {
        //        db.Entry(menu).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //    }
        //    catch { }
        //    return RedirectToAction("Menu");
        //}

        //[HttpPost]
        //public async Task<IActionResult> DeleteMenu(int id)
        //{
        //    try
        //    {
        //        var menu = await db.Menus.FirstOrDefaultAsync(x => x.id == id);
        //        db.Menus.Remove(menu);
        //        await db.SaveChangesAsync();
        //    }
        //    catch { }
        //    return RedirectToAction("Menu");
        //}
        //#endregion
    }
}
