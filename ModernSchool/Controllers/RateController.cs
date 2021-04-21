using Microsoft.AspNetCore.Mvc;
using ModernSchool.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Controllers
{
    public class RateController : Controller
    {
        private DataContext db;

        public RateController(DataContext context)
        {
            db = context;
        }

        public IActionResult SelectSchool()
        {
            return View();
        }
    }
}
