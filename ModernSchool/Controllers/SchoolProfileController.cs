using Microsoft.AspNetCore.Authorization;
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
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ModernSchool.Controllers
{
    [Authorize(Roles = "5")]
    public class SchoolProfileController : Controller
    {
        public static int _year;

        private DataContext db;
        private DataManager data;
        readonly IWebHostEnvironment _appEnvironment;
        
        public SchoolProfileController(DataContext context, IWebHostEnvironment hostEnvironment)
        {
            db = context;
            data = new DataManager(db);
            _appEnvironment = hostEnvironment;
            _year = db.CurrentYear.First().Year;
        }
        
        public async Task<IActionResult> Profile()
        {
            int school_id = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value);
            return View(await db.Schools.Include(x => x.Region).Include(x => x.District).Include(x => x.SchoolType).FirstOrDefaultAsync(x => x.Id == school_id));
        }

        public async Task<IActionResult> EditProfile()
        {
            int school_id = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value);
            return View(await db.Schools.Include(x => x.Region).Include(x => x.District).Include(x => x.SchoolType).FirstOrDefaultAsync(x => x.Id == school_id));
        }
        
        [HttpPost]
        public async Task<IActionResult> EditProfile(School school)
        {
            try
            {
                school.UpdateDate = DateTime.Now;
                db.Entry(school).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("Profile");
        }

        public async Task<IActionResult> UserProfile()
        {
            int school_id = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value);
            return View(await db.Users.FirstOrDefaultAsync(x=>x.SchoolId == school_id));
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateUserProfile(User user)
        {
            try
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("UserProfile");
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateUserPassword(User user)
        {
            try
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("UserProfile");
        }

        public async Task<IActionResult> Indexes()
        {
            int school_id = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value);
            PageData pageData = new();
            pageData.Rates = await db.Rates.Where(x => x.SchoolId == school_id && x.Year == _year).ToListAsync();
            pageData.UploadFiles = await db.UploadFiles.Where(x => x.SchoolId == school_id && x.Year == _year).ToListAsync();
            pageData.Criterias = await db.Criterias.ToListAsync();
            pageData.Indexes = await db.Indexes.Include(x=>x.Criterias).ToListAsync();
            pageData.IndexesDataStatuses = await data.IndexesStatus(school_id, _year);
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
                    double ValueSchool = Convert.ToDouble(temp[1]);
                }

                foreach (var item in criteriaValues)
                {
                    var temp = item.Split(';');

                    int CriteriaId = Convert.ToInt32(temp[0]);
                    double ValueSchool = Convert.ToDouble(temp[1]);

                    if (CriteriaId == 186 || CriteriaId == 188 || CriteriaId == 190)
                    {
                        List<Rate> _rates = await db.Rates.Where(x => (CriteriaId == 186 || CriteriaId == 188 || CriteriaId == 190) && x.ValueSchool != null && x.Year != _year).ToListAsync();
                        if (_rates != null)
                        {
                            db.Rates.RemoveRange(_rates);
                            await db.SaveChangesAsync();
                        }
                    }

                    if (CriteriaId == 181 || CriteriaId == 209)
                    {
                        List<Rate> _rates = await db.Rates.Where(x => (CriteriaId == 181 || CriteriaId == 209) && x.ValueSchool != null && x.Year != _year).ToListAsync();
                        if (_rates != null)
                        {
                            db.Rates.RemoveRange(_rates);
                            await db.SaveChangesAsync();
                        }
                    }

                    if (CriteriaId == 154 || CriteriaId == 160 || CriteriaId == 167)
                    {
                        List<Rate> _rates = await db.Rates.Where(x => (CriteriaId == 154 || CriteriaId == 160 || CriteriaId == 167) && x.ValueSchool != null && x.Year != _year).ToListAsync();
                        if (_rates != null)
                        {
                            db.Rates.RemoveRange(_rates);
                            await db.SaveChangesAsync();
                        }
                    }

                    if (CriteriaId == 149 || CriteriaId == 179)
                    {
                        List<Rate> _rates = await db.Rates.Where(x => (CriteriaId == 149 || CriteriaId == 179) && x.ValueSchool != null && x.Year != _year).ToListAsync();
                        if (_rates != null)
                        {
                            db.Rates.RemoveRange(_rates);
                            await db.SaveChangesAsync();
                        }
                    }

                    if (CriteriaId == 94 || CriteriaId == 102)
                    {
                        List<Rate> _rates = await db.Rates.Where(x => (CriteriaId == 94 || CriteriaId == 102) && x.ValueSchool != null && x.Year != _year).ToListAsync();
                        if (_rates != null)
                        {
                            db.Rates.RemoveRange(_rates);
                            await db.SaveChangesAsync();
                        }
                    }

                    if (CriteriaId == 106 || CriteriaId == 110 || CriteriaId == 148 || CriteriaId == 168 || CriteriaId == 170 || CriteriaId == 177 || CriteriaId == 271)
                    {
                        List<Rate> _rates = await db.Rates.Where(x => (CriteriaId == 106 || CriteriaId == 110 || CriteriaId == 148 || CriteriaId == 168 || CriteriaId == 170 || CriteriaId == 177 || CriteriaId == 271) && x.ValueSchool != null && x.Year != _year).ToListAsync();
                        if (_rates != null)
                        {
                            db.Rates.RemoveRange(_rates);
                            await db.SaveChangesAsync();
                        }
                    }
                }


                List<Rate> rates = await db.Rates.Where(x => x.IndexId == indexId && x.ValueSchool != null && x.Year != _year).ToListAsync();
                if (rates != null)
                {
                    db.Rates.RemoveRange(rates);
                    await db.SaveChangesAsync();
                }
                foreach (var item in criteriaValues)
                {
                    var temp = item.Split(';');

                    int CriteriaId = Convert.ToInt32(temp[0]);
                    double ValueSchool = Convert.ToDouble(temp[1]);

                    if (CriteriaId == 186 || CriteriaId == 188 || CriteriaId == 190)
                    {
                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 140,
                            CriteriaId = 186,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 141,
                            CriteriaId = 188,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 142,
                            CriteriaId = 190,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,

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
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 117,
                            CriteriaId = 209,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,

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
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 105,
                            CriteriaId = 160,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 107,
                            CriteriaId = 167,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,

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
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 101,
                            CriteriaId = 179,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,

                        });
                        await db.SaveChangesAsync();
                    }

                    else if (CriteriaId == 94 || CriteriaId == 102)
                    {
                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 86,
                            CriteriaId = 94,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 87,
                            CriteriaId = 102,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,

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
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 89,
                            CriteriaId = 110,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 99,
                            CriteriaId = 148,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 107,
                            CriteriaId = 168,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 108,
                            CriteriaId = 170,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 110,
                            CriteriaId = 177,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 94,
                            CriteriaId = 271,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,

                        });
                        await db.SaveChangesAsync();
                    }

                    else
                    {
                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = indexId,
                            CriteriaId = Convert.ToInt32(temp[0]),
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = _year,
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
                uploadFile.CreatedBy = 1;
                uploadFile.Year = _year;
                uploadFile.FileExtension = extension;
                uploadFile.FileGuid = fileGuidName;
                uploadFile.FileName = MyUploader.FileName;
                uploadFile.IndexId = IndexId;
                uploadFile.SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value);

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

        public async Task<int> SolveIndexBall()
        {
            int school_id = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value);
            var rates = await db.Rates.Where(x => x.SchoolId == school_id && x.ValueSchool != null && x.Year == _year).ToListAsync();
            
            //Respublika Olimpiada
            try
            {
                double? i = ((10 * rates.FirstOrDefault(x => x.CriteriaId == 79).ValueSchool + 5 * rates.FirstOrDefault(x => x.CriteriaId == 80).ValueSchool + 3 * rates.FirstOrDefault(x => x.CriteriaId == 81).ValueSchool) 
                    + (15 * rates.FirstOrDefault(x => x.CriteriaId == 82).ValueSchool + 13 * rates.FirstOrDefault(x => x.CriteriaId == 83).ValueSchool + 10 * rates.FirstOrDefault(x => x.CriteriaId == 84).ValueSchool)
                    + (20 * rates.FirstOrDefault(x => x.CriteriaId == 85).ValueSchool + 18 * rates.FirstOrDefault(x => x.CriteriaId == 86).ValueSchool + 15 * rates.FirstOrDefault(x => x.CriteriaId == 87).ValueSchool))
                    / (rates.FirstOrDefault(x => x.CriteriaId == 106).ValueSchool <= 630 ? 9 : rates.FirstOrDefault(x => x.CriteriaId == 106).ValueSchool <= 945 ? 18 : 27);

                var maxball = await data.MaxBallInRepublicOlympiads(_year);

                double itogBall = 0;
                if (i >= maxball)
                {
                    itogBall = 20;
                }
                else
                {
                    itogBall = (double)((20 * i) / maxball);
                }

                try
                {
                    var res = db.IndexBalls.FirstOrDefault(x => x.IndexId == 84 && x.SchoolId == school_id && x.Year == _year);
                    if (res != null)
                    {
                        if (res.SchoolBall != itogBall)
                        {
                            res.SchoolBall = itogBall;
                            db.Entry(res).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        await db.IndexBalls.AddAsync(new IndexBall { 
                            IndexId = 84,
                            SchoolId = school_id,
                            Year = _year,
                            SchoolBall = itogBall
                        });
                        await db.SaveChangesAsync();
                    }
                }
                catch { }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //Xalqaro Olimpiada
            try
            {
                double? i = 5 * rates.FirstOrDefault(x => x.CriteriaId == 89).ValueSchool + 10 * rates.FirstOrDefault(x => x.CriteriaId == 90).ValueSchool + 20 * rates.FirstOrDefault(x => x.CriteriaId == 91).ValueSchool + 25 * rates.FirstOrDefault(x => x.CriteriaId == 92).ValueSchool;

                var maxball = await data.MaxBallInInternationalOlympiads(_year);

                double itogBall = 0;
                if (i >= maxball)
                {
                    itogBall = 25;
                }
                else
                {
                    itogBall = (double)((25 * i) / maxball);
                }

                try
                {
                    var res = db.IndexBalls.FirstOrDefault(x => x.IndexId == 85 && x.SchoolId == school_id && x.Year == _year);
                    if (res != null)
                    {
                        if (res.SchoolBall != itogBall)
                        {
                            res.SchoolBall = itogBall;
                            db.Entry(res).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        await db.IndexBalls.AddAsync(new IndexBall
                        {
                            IndexId = 85,
                            SchoolId = school_id,
                            Year = _year,
                            SchoolBall = itogBall
                        });
                        await db.SaveChangesAsync();
                    }
                }
                catch { }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            //Oqishga kirish
            try
            {
                double? i = rates.FirstOrDefault(x => x.CriteriaId == 93).ValueSchool / rates.FirstOrDefault(x => x.CriteriaId == 94).ValueSchool;

                var maxball = await data.MaxBallInAbitur(_year);

                double itogBall = 0;
                if (i >= maxball)
                {
                    itogBall = 25;
                }
                else
                {
                    itogBall = (double)((25 * i) / maxball);
                }

                try
                {
                    var res = db.IndexBalls.FirstOrDefault(x => x.IndexId == 86 && x.SchoolId == school_id && x.Year == _year);
                    if (res != null)
                    {
                        if (res.SchoolBall != itogBall)
                        {
                            res.SchoolBall = itogBall;
                            db.Entry(res).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        await db.IndexBalls.AddAsync(new IndexBall
                        {
                            IndexId = 86,
                            SchoolId = school_id,
                            Year = _year,
                            SchoolBall = itogBall
                        });
                        await db.SaveChangesAsync();
                    }
                }
                catch { }
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

                var maxball = await data.MaxBallInBandlik(_year);

                double itogBall = 0;
                if (i >= maxball)
                {
                    itogBall = 100;
                }
                else
                {
                    itogBall = (double)((100 * i) / maxball);
                }

                try
                {
                    var res = db.IndexBalls.FirstOrDefault(x => x.IndexId == 87 && x.SchoolId == school_id && x.Year == _year);
                    if (res != null)
                    {
                        if (res.SchoolBall != itogBall)
                        {
                            res.SchoolBall = itogBall;
                            db.Entry(res).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        await db.IndexBalls.AddAsync(new IndexBall
                        {
                            IndexId = 87,
                            SchoolId = school_id,
                            Year = _year,
                            SchoolBall = itogBall
                        });
                        await db.SaveChangesAsync();
                    }
                }
                catch { }
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

                var maxball = await data.MaxBallInRespublikaTanlov(_year);

                double itogBall = 0;
                if (i >= maxball)
                {
                    itogBall = 15;
                }
                else
                {
                    itogBall = (double)((15 * i) / maxball);
                }

                try
                {
                    var res = db.IndexBalls.FirstOrDefault(x => x.IndexId == 88 && x.SchoolId == school_id && x.Year == _year);
                    if (res != null)
                    {
                        if (res.SchoolBall != itogBall)
                        {
                            res.SchoolBall = itogBall;
                            db.Entry(res).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        await db.IndexBalls.AddAsync(new IndexBall
                        {
                            IndexId = 88,
                            SchoolId = school_id,
                            Year = _year,
                            SchoolBall = itogBall
                        });
                        await db.SaveChangesAsync();
                    }
                }
                catch { }
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

                var maxball = await data.MaxBallInXalqaroTanlov(_year);

                double itogBall = 0;
                if (i >= maxball)
                {
                    itogBall = 25;
                }
                else
                {
                    itogBall = (double)((25 * i) / maxball);
                }

                try
                {
                    var res = db.IndexBalls.FirstOrDefault(x => x.IndexId == 89 && x.SchoolId == school_id && x.Year == _year);
                    if (res != null)
                    {
                        if (res.SchoolBall != itogBall)
                        {
                            res.SchoolBall = itogBall;
                            db.Entry(res).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        await db.IndexBalls.AddAsync(new IndexBall
                        {
                            IndexId = 89,
                            SchoolId = school_id,
                            Year = _year,
                            SchoolBall = itogBall
                        });
                        await db.SaveChangesAsync();
                    }
                }
                catch { }
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

                try
                {
                    var res = db.IndexBalls.FirstOrDefault(x => x.IndexId == 94 && x.SchoolId == school_id && x.Year == _year);
                    if (res != null)
                    {
                        if (res.SchoolBall != itogBall)
                        {
                            res.SchoolBall = itogBall;
                            db.Entry(res).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        await db.IndexBalls.AddAsync(new IndexBall
                        {
                            IndexId = 94,
                            SchoolId = school_id,
                            Year = _year,
                            SchoolBall = itogBall
                        });
                        await db.SaveChangesAsync();
                    }
                }
                catch { }
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
                

                try
                {
                    var res = db.IndexBalls.FirstOrDefault(x => x.IndexId == 96 && x.SchoolId == school_id && x.Year == _year);
                    if (res != null)
                    {
                        if (res.SchoolBall != itogBall)
                        {
                            res.SchoolBall = itogBall;
                            db.Entry(res).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        await db.IndexBalls.AddAsync(new IndexBall
                        {
                            IndexId = 96,
                            SchoolId = school_id,
                            Year = _year,
                            SchoolBall = itogBall
                        });
                        await db.SaveChangesAsync();
                    }
                }
                catch { }
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


                try
                {
                    var res = db.IndexBalls.FirstOrDefault(x => x.IndexId == 99 && x.SchoolId == school_id && x.Year == _year);
                    if (res != null)
                    {
                        if (res.SchoolBall != itogBall)
                        {
                            res.SchoolBall = itogBall;
                            db.Entry(res).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        await db.IndexBalls.AddAsync(new IndexBall
                        {
                            IndexId = 99,
                            SchoolId = school_id,
                            Year = _year,
                            SchoolBall = itogBall
                        });
                        await db.SaveChangesAsync();
                    }
                }
                catch { }
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

                try
                {
                    var res = db.IndexBalls.FirstOrDefault(x => x.IndexId == 100 && x.SchoolId == school_id && x.Year == _year);
                    if (res != null)
                    {
                        if (res.SchoolBall != itogBall)
                        {
                            res.SchoolBall = itogBall;
                            db.Entry(res).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        await db.IndexBalls.AddAsync(new IndexBall
                        {
                            IndexId = 100,
                            SchoolId = school_id,
                            Year = _year,
                            SchoolBall = itogBall
                        });
                        await db.SaveChangesAsync();
                    }
                }
                catch { }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return 1;
        }



        #region comment 
        public async Task<IActionResult> MainInfo()
        {
            int school_id = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value);
            return View(await db.Schools.Include(x => x.Region).Include(x => x.District).Include(x => x.SchoolType).Include(x => x.SchoolInfo).FirstOrDefaultAsync(x => x.Id == school_id));
        }
        [HttpPost]
        public async Task<IActionResult> MainInfo(SchoolInfo schoolInfo)
        {
            try
            {
                schoolInfo.update_date = DateTime.Now;
                schoolInfo.year = DateTime.Now.Year;
                if (schoolInfo.id != 0)
                    db.Entry(schoolInfo).State = EntityState.Modified;
                else
                    db.SchoolInfos.Add(schoolInfo);
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("MainInfo");
        }
        #endregion
    }
}
