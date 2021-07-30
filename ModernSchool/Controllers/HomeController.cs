using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModernSchool.DB;
using ModernSchool.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ModernSchool.Controllers
{
    [Authorize(Roles = "1")]
    public class HomeController : Controller
    {
        private DataContext db;
        private DataManager data;

        public HomeController(DataContext context)
        {
            db = context;
            data = new DataManager(db);
        }

        public async Task<IActionResult> Index()
        {
            PageData pageData = new();
            pageData.Rates = await db.Rates.Where(x => x.SchoolId == 5663).ToListAsync();
            pageData.UploadFiles = await db.UploadFiles.Where(x => x.SchoolId == 5663).ToListAsync();
            pageData.Criterias = await db.Criterias.ToListAsync();
            pageData.Indexes = await db.Indexes.Include(x => x.Criterias).ToListAsync();
            pageData.IndexesDataStatuses = await data.IndexesStatus(5663);
            return View(pageData);
        }

        public IActionResult Profile()
        {
            string login = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            var user = db.Users.Include(x=>x.Role).FirstOrDefault(x => x.Login == login);
            return View(user);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [AllowAnonymous]
        public IActionResult ChangeCulture(string lang)
        {
            Uri baseUri = new Uri(Request.Headers["Referer"].ToString());
            Response.Cookies.Append("lang", lang);
            return Redirect(baseUri.AbsolutePath);
        }
    }
}
