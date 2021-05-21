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

        public SchoolProfileController(DataContext context)
        {
            db = context;
        }
        public IActionResult Profile()
        {
            int school_id = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value);
            return View(db.Schools.Include(x => x.Region).Include(x => x.District).Include(x => x.SchoolType).FirstOrDefault(x => x.Id == school_id));
        }

        public IActionResult EditProfile()
        {
            int school_id = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value);
            return View(db.Schools.Include(x => x.Region).Include(x => x.District).Include(x => x.SchoolType).FirstOrDefault(x => x.Id == school_id));
        }
        [HttpPost]
        public IActionResult EditProfile(School school)
        {
            try
            {
                school.UpdateDate = DateTime.Now;
                db.Entry(school).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Profile");
        }

        public IActionResult UserProfile()
        {
            int school_id = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value);
            return View(db.Users.FirstOrDefault(x=>x.SchoolId == school_id));
        }
        [HttpPost]
        public IActionResult UpdateUserProfile(User user)
        {
            try
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("UserProfile");
        }
        [HttpPost]
        public IActionResult UpdateUserPassword(User user)
        {
            try
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("UserProfile");
        }

        public IActionResult Info()
        {
            return View();
        }

        public IActionResult MainInfo()
        {
            int school_id = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value);
            return View(db.Schools.Include(x => x.Region).Include(x => x.District).Include(x => x.SchoolType).Include(x=>x.SchoolInfo).FirstOrDefault(x => x.Id == school_id));
        }
        [HttpPost]
        public IActionResult MainInfo(SchoolInfo schoolInfo)
        {
            try
            {
                schoolInfo.update_date = DateTime.Now;
                schoolInfo.year = DateTime.Now.Year;
                if (schoolInfo.id != 0)
                    db.Entry(schoolInfo).State = EntityState.Modified;
                else
                    db.SchoolInfos.Add(schoolInfo);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("MainInfo");
        }
        public IActionResult TeachersInfo()
        {
            int school_id = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value);
            return View(db.Schools.Include(x => x.SchoolInfo).Include(x=>x.TeacherInfo).FirstOrDefault(x => x.Id == school_id));
        }
        [HttpPost]
        public IActionResult TeachersInfo(TeacherInfo teacherInfo)
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
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("TeachersInfo");
        }

        public IActionResult PupilsInfo()
        {
            int school_id = Convert.ToInt32(User.FindFirst(x => x.Type == "SchoolId").Value);
            var model = db.Schools.Include(x => x.SchoolInfo).Include(x => x.PupilInfo).Include(x => x.InternationOlympiadWinners).Include(x => x.RepublicOlympiadWinners).FirstOrDefault(x => x.Id == school_id);
            model.Subjects = db.Subjects.ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult PupilsInfo(PupilInfo pupilInfo)
        {
            try
            {
                pupilInfo.update_date = DateTime.Now;
                pupilInfo.year = DateTime.Now.Year;
                if (pupilInfo.id != 0)
                    db.Entry(pupilInfo).State = EntityState.Modified;
                else
                    db.PupilInfos.Add(pupilInfo);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("PupilsInfo");
        }
        [HttpPost]
        public IActionResult SaveRepublicOlympiadWinners(RepublicOlympiadWinner republicOlympiadWinner)
        {
            try
            {
                republicOlympiadWinner.update_date = DateTime.Now;
                republicOlympiadWinner.year = DateTime.Now.Year;
                db.RepublicOlympiadWinners.Add(republicOlympiadWinner);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("PupilsInfo");
        }
        [HttpPost]
        public IActionResult SaveInternationOlympiadWinners(InternationOlympiadWinner internationOlympiadWinner)
        {
            try
            {
                internationOlympiadWinner.update_date = DateTime.Now;
                internationOlympiadWinner.year = DateTime.Now.Year;
                db.InternationOlympiadWinners.Add(internationOlympiadWinner);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("PupilsInfo");
        }

        [HttpPost]
        public IActionResult DeleteRepublicOlympiadWinners(int id)
        {
            try
            {
                var data = db.RepublicOlympiadWinners.FirstOrDefault(x => x.id == id);
                db.RepublicOlympiadWinners.Remove(data);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("PupilsInfo");
        }
        [HttpPost]
        public IActionResult DeleteInternationOlympiadWinners(int id)
        {
            try
            {
                var data = db.InternationOlympiadWinners.FirstOrDefault(x => x.id == id);
                db.InternationOlympiadWinners.Remove(data);
                db.SaveChanges();
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
            pageData.Indexes = db.Indexes;
            pageData.SchoolMenus = db.SchoolMenus.Include(x=>x.Menu).Include(x=>x.Criteria).Include(x=>x.Criteria.Index).Where(x => x.menu_id == menu_id);
            return View(pageData);
        }
        [HttpPost]
        public JsonResult SaveQuestionnaire(string[] criteriaValues)
        {
            var result = 0;

            try
            {
                foreach (var item in criteriaValues)
                {
                    var temp = item.Split(';');
                    var criteria = db.Criterias.FirstOrDefault(x => x.Id == Convert.ToInt32(temp[0]));
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
                    db.SaveChanges();
                }
                result = 1;
            }
            catch (Exception e)
            {
                var r = e.Message;
            }

            return Json(result);
        }
    }
}
