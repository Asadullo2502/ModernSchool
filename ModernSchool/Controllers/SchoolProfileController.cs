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
    }
}
