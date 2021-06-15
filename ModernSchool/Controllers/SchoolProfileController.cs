using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModernSchool.DB;
using ModernSchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Controllers
{
    [Authorize(Roles = "5")]
    public class SchoolProfileController : Controller
    {
        private DataContext db;
        private DataManager data;
        public SchoolProfileController(DataContext context)
        {
            db = context;
            data = new DataManager(db);
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

        public IActionResult Info()
        {
            return View();
        }

        public async Task<IActionResult> Indexes(int menu_id)
        {
            int school_id = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value);
            PageData pageData = new();
            pageData.Rates = await db.Rates.Where(x => x.SchoolId == school_id).ToListAsync();
            pageData.Criterias = await db.Criterias.ToListAsync();
            pageData.Indexes = await db.Indexes.Include(x=>x.Criterias).ToListAsync();
            pageData.IndexesDataStatuses = await data.IndexesStatus();
            return View(pageData);
        }

        [HttpPost]
        public async Task<JsonResult> SaveIndex(int indexId, string[] criteriaValues)
        {
            var result = 0;

            try
            {
                List<Rate> rates = await db.Rates.Where(x => x.IndexId == indexId).ToListAsync();
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
                        ValueSchool = Convert.ToDouble(temp[1]),
                        SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
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
        public async Task<IActionResult> TeachersInfo()
        {
            int school_id = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value);
            return View(await db.Schools.Include(x => x.SchoolInfo).Include(x => x.TeacherInfo).FirstOrDefaultAsync(x => x.Id == school_id));
        }
        [HttpPost]
        public async Task<IActionResult> TeachersInfo(TeacherInfo teacherInfo)
        {
            try
            {
                var tyutors_existence = Request.Form["tyutors_existence"];
                teacherInfo.tyutors_existence = tyutors_existence == "on" ? true : false;
                teacherInfo.update_date = DateTime.Now;
                teacherInfo.year = DateTime.Now.Year;
                if (teacherInfo.id != 0)
                    db.Entry(teacherInfo).State = EntityState.Modified;
                else
                    db.TeacherInfos.Add(teacherInfo);
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("TeachersInfo");
        }

        public async Task<IActionResult> PupilsInfo()
        {
            int school_id = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value);
            var model = await db.Schools.Include(x => x.SchoolInfo).Include(x => x.PupilInfo).Include(x => x.InternationOlympiadWinners).Include(x => x.RepublicOlympiadWinners).FirstOrDefaultAsync(x => x.Id == school_id);
            model.Subjects = await db.Subjects.ToListAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> PupilsInfo(PupilInfo pupilInfo)
        {
            try
            {
                pupilInfo.update_date = DateTime.Now;
                pupilInfo.year = DateTime.Now.Year;
                if (pupilInfo.id != 0)
                    db.Entry(pupilInfo).State = EntityState.Modified;
                else
                    db.PupilInfos.Add(pupilInfo);
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("PupilsInfo");
        }
        [HttpPost]
        public async Task<IActionResult> SaveRepublicOlympiadWinners(RepublicOlympiadWinner republicOlympiadWinner)
        {
            try
            {
                republicOlympiadWinner.update_date = DateTime.Now;
                republicOlympiadWinner.year = DateTime.Now.Year;
                db.RepublicOlympiadWinners.Add(republicOlympiadWinner);
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("PupilsInfo");
        }
        [HttpPost]
        public async Task<IActionResult> SaveInternationOlympiadWinners(InternationOlympiadWinner internationOlympiadWinner)
        {
            try
            {
                internationOlympiadWinner.update_date = DateTime.Now;
                internationOlympiadWinner.year = DateTime.Now.Year;
                db.InternationOlympiadWinners.Add(internationOlympiadWinner);
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("PupilsInfo");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRepublicOlympiadWinners(int id)
        {
            try
            {
                var data = await db.RepublicOlympiadWinners.FirstOrDefaultAsync(x => x.id == id);
                db.RepublicOlympiadWinners.Remove(data);
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("PupilsInfo");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteInternationOlympiadWinners(int id)
        {
            try
            {
                var data = await db.InternationOlympiadWinners.FirstOrDefaultAsync(x => x.id == id);
                db.InternationOlympiadWinners.Remove(data);
                await db.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction("PupilsInfo");
        }

        public IActionResult Questionnaire(int menu_id)
        {
            int school_id = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value);
            PageData pageData = new PageData();
            pageData.Rates = db.Rates.Where(x => x.SchoolId == school_id);
            pageData.Criterias = db.Criterias;
            pageData.SchoolMenus = db.SchoolMenus.Include(x => x.Criteria.Index).Include(x => x.Menu).Where(x => x.menu_id == menu_id);

            return View(pageData);
        }
        [HttpPost]
        public async Task<JsonResult> SaveQuestionnaire(string[] criteriaValues)
        {
            var result = 0;

            try
            {
                foreach (var item in criteriaValues)
                {
                    var temp = item.Split(';');
                    var criteria = await db.Criterias.FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(temp[0]));
                    List<Rate> rates = db.Rates.Where(x => x.IndexId == criteria.IndexId).ToList();
                    if (rates != null)
                    {
                        db.Rates.RemoveRange(rates);
                        db.SaveChanges();
                    }
                }

                foreach (var item in criteriaValues)
                {
                    var temp = item.Split(';');

                    var rate = new Rate
                    {
                        UpdateDateSchool = DateTime.Now,
                        IndexId = db.Criterias.FirstOrDefault(x => x.Id == Convert.ToInt32(temp[0])).IndexId,
                        CriteriaId = Convert.ToInt32(temp[0]),
                        ValueSchool = Convert.ToDouble(temp[1]),
                        SchoolId = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value),
                        Year = 2021
                    };

                    db.Rates.Add(rate);
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
        #endregion
    }
}
