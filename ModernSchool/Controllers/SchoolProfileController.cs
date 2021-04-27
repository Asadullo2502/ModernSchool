using Microsoft.AspNetCore.Mvc;
using ModernSchool.DB;
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
        public IActionResult Index(int? id = 1)
        {
            return View(db.Schools.Find(id));
        }
    }
}
