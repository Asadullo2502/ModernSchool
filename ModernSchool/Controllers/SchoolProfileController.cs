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
        private DataContext db;
        private DataManager data;
        readonly IWebHostEnvironment _appEnvironment;
        
        public SchoolProfileController(DataContext context, IWebHostEnvironment hostEnvironment)
        {
            db = context;
            data = new DataManager(db);
            _appEnvironment = hostEnvironment;
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
            pageData.Rates = await db.Rates.Where(x => x.SchoolId == school_id).ToListAsync();
            pageData.UploadFiles = await db.UploadFiles.Where(x => x.SchoolId == school_id).ToListAsync();
            pageData.Criterias = await db.Criterias.ToListAsync();
            pageData.Indexes = await db.Indexes.Include(x=>x.Criterias).ToListAsync();
            pageData.IndexesDataStatuses = await data.IndexesStatus(school_id);
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
                        List<Rate> _rates = await db.Rates.Where(x => (CriteriaId == 186 || CriteriaId == 188 || CriteriaId == 190) && x.ValueSchool != null).ToListAsync();
                        if (_rates != null)
                        {
                            db.Rates.RemoveRange(_rates);
                            await db.SaveChangesAsync();
                        }
                    }

                    if (CriteriaId == 181 || CriteriaId == 209)
                    {
                        List<Rate> _rates = await db.Rates.Where(x => (CriteriaId == 181 || CriteriaId == 209) && x.ValueSchool != null).ToListAsync();
                        if (_rates != null)
                        {
                            db.Rates.RemoveRange(_rates);
                            await db.SaveChangesAsync();
                        }
                    }

                    if (CriteriaId == 154 || CriteriaId == 160 || CriteriaId == 167)
                    {
                        List<Rate> _rates = await db.Rates.Where(x => (CriteriaId == 154 || CriteriaId == 160 || CriteriaId == 167) && x.ValueSchool != null).ToListAsync();
                        if (_rates != null)
                        {
                            db.Rates.RemoveRange(_rates);
                            await db.SaveChangesAsync();
                        }
                    }

                    if (CriteriaId == 149 || CriteriaId == 179)
                    {
                        List<Rate> _rates = await db.Rates.Where(x => (CriteriaId == 149 || CriteriaId == 179) && x.ValueSchool != null).ToListAsync();
                        if (_rates != null)
                        {
                            db.Rates.RemoveRange(_rates);
                            await db.SaveChangesAsync();
                        }
                    }

                    if (CriteriaId == 106 || CriteriaId == 110 || CriteriaId == 148 || CriteriaId == 168 || CriteriaId == 170 || CriteriaId == 177 || CriteriaId == 271)
                    {
                        List<Rate> _rates = await db.Rates.Where(x => (CriteriaId == 106 || CriteriaId == 110 || CriteriaId == 148 || CriteriaId == 168 || CriteriaId == 170 || CriteriaId == 177 || CriteriaId == 271) && x.ValueSchool != null).ToListAsync();
                        if (_rates != null)
                        {
                            db.Rates.RemoveRange(_rates);
                            await db.SaveChangesAsync();
                        }
                    }
                }


                List<Rate> rates = await db.Rates.Where(x => x.IndexId == indexId && x.ValueSchool != null).ToListAsync();
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
                            Year = 2021,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 141,
                            CriteriaId = 188,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = 2021,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 142,
                            CriteriaId = 190,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = 2021,

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
                            Year = 2021,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 117,
                            CriteriaId = 209,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = 2021,

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
                            Year = 2021,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 105,
                            CriteriaId = 160,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = 2021,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 107,
                            CriteriaId = 167,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = 2021,

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
                            Year = 2021,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 101,
                            CriteriaId = 179,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = 2021,

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
                            Year = 2021,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 89,
                            CriteriaId = 110,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = 2021,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 99,
                            CriteriaId = 148,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = 2021,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 107,
                            CriteriaId = 168,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = 2021,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 108,
                            CriteriaId = 170,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = 2021,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 110,
                            CriteriaId = 177,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = 2021,

                        });
                        await db.SaveChangesAsync();

                        await db.Rates.AddAsync(new Rate
                        {
                            UpdateDateSchool = DateTime.Now,
                            IndexId = 94,
                            CriteriaId = 271,
                            ValueSchool = Convert.ToDouble(temp[1]),
                            SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                            Year = 2021,

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
                            Year = 2021,
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
