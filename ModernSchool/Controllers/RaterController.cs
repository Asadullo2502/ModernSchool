using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModernSchool.DB;
using ModernSchool.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Controllers
{
    public class RaterController : Controller
    {
        public static int _year;

        private DataContext db;
        private DataManager data;
        readonly IWebHostEnvironment _appEnvironment;

        public RaterController(DataContext context, IWebHostEnvironment hostEnvironment)
        {
            db = context;
            data = new DataManager(db);
            _appEnvironment = hostEnvironment;
            _year = db.CurrentYear.First().Year;
        }

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
            return View(schools.Take(0).ToList());
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
            pageData.IndexesDataStatuses = await data.IndexesStatus((int)id, _year);
            return View(pageData);
        }

        [HttpPost]
        public async Task<JsonResult> SaveIndex(int indexId, string[] criteriaValues)
        {
            var result = 0;

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
                        List<Rate> _rates = await db.Rates.Where(x => (CriteriaId == 186 || CriteriaId == 188 || CriteriaId == 190) && x.ValueInspektor != null).ToListAsync();
                        if (_rates != null)
                        {
                            db.Rates.RemoveRange(_rates);
                            await db.SaveChangesAsync();
                        }
                    }

                    if (CriteriaId == 181 || CriteriaId == 209)
                    {
                        List<Rate> _rates = await db.Rates.Where(x => (CriteriaId == 181 || CriteriaId == 209) && x.ValueInspektor != null).ToListAsync();
                        if (_rates != null)
                        {
                            db.Rates.RemoveRange(_rates);
                            await db.SaveChangesAsync();
                        }
                    }

                    if (CriteriaId == 154 || CriteriaId == 160 || CriteriaId == 167)
                    {
                        List<Rate> _rates = await db.Rates.Where(x => (CriteriaId == 154 || CriteriaId == 160 || CriteriaId == 167) && x.ValueInspektor != null).ToListAsync();
                        if (_rates != null)
                        {
                            db.Rates.RemoveRange(_rates);
                            await db.SaveChangesAsync();
                        }
                    }

                    if (CriteriaId == 149 || CriteriaId == 179)
                    {
                        List<Rate> _rates = await db.Rates.Where(x => (CriteriaId == 149 || CriteriaId == 179) && x.ValueInspektor != null).ToListAsync();
                        if (_rates != null)
                        {
                            db.Rates.RemoveRange(_rates);
                            await db.SaveChangesAsync();
                        }
                    }

                    if (CriteriaId == 106 || CriteriaId == 110 || CriteriaId == 148 || CriteriaId == 168 || CriteriaId == 170 || CriteriaId == 177 || CriteriaId == 271)
                    {
                        List<Rate> _rates = await db.Rates.Where(x => (CriteriaId == 106 || CriteriaId == 110 || CriteriaId == 148 || CriteriaId == 168 || CriteriaId == 170 || CriteriaId == 177 || CriteriaId == 271) && x.ValueInspektor != null).ToListAsync();
                        if (_rates != null)
                        {
                            db.Rates.RemoveRange(_rates);
                            await db.SaveChangesAsync();
                        }
                    }
                }

                List<Rate> rates = await db.Rates.Where(x => x.IndexId == indexId && x.ValueInspektor != null).ToListAsync();
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
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 140,
                            CriteriaId = 186,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 141,
                            CriteriaId = 188,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 142,
                            CriteriaId = 190,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();
                    }

                    else if (CriteriaId == 181 || CriteriaId == 209)
                    {
                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 101,
                            CriteriaId = 181,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 117,
                            CriteriaId = 209,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();
                    }

                    else if (CriteriaId == 154 || CriteriaId == 160 || CriteriaId == 167)
                    {
                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 103,
                            CriteriaId = 154,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 105,
                            CriteriaId = 160,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 107,
                            CriteriaId = 167,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();
                    }

                    else if (CriteriaId == 149 || CriteriaId == 179)
                    {
                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 100,
                            CriteriaId = 149,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 101,
                            CriteriaId = 179,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();
                    }

                    else if (CriteriaId == 106 || CriteriaId == 110 || CriteriaId == 148 || CriteriaId == 168 || CriteriaId == 170 || CriteriaId == 177 || CriteriaId == 271)
                    {
                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 88,
                            CriteriaId = 106,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 89,
                            CriteriaId = 110,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 99,
                            CriteriaId = 148,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 107,
                            CriteriaId = 168,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 108,
                            CriteriaId = 170,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 110,
                            CriteriaId = 177,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,
                            InspektorId = 1
                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 94,
                            CriteriaId = 271,
                            ValueInspektor = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
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
                            SchoolId = Convert.ToInt32(Request.Cookies["school_id"]),
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
                uploadFile.CreatedBy = _year;
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
    }
}
