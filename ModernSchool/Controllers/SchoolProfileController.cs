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
    public class SchoolProfileController : Controller
    {
        private DataContext db;

        public SchoolProfileController(DataContext context)
        {
            db = context;
        }
        public IActionResult Profile(int? id = 1)
        {
            return View(db.Schools.Include(x => x.Region).Include(x => x.District).Include(x => x.SchoolType).FirstOrDefault(x => x.Id == id));
        }

        public IActionResult EditProfile(int? id = 1)
        {
            return View(db.Schools.Include(x => x.Region).Include(x => x.District).Include(x => x.SchoolType).FirstOrDefault(x => x.Id == id));
        }
        [HttpPost]
        public IActionResult EditProfile(School school)
        {
            try
            {
                db.Entry(school).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Profile");
        }

        public IActionResult UserProfile(int? id = 1)
        {
            return View(db.Users.FirstOrDefault(x=>x.SchoolId == id));
        }
    }
}
