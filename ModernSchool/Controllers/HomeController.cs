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
        private readonly ILogger<HomeController> _logger;
        private DataContext db;
        private DataManager data;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
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

        [HttpPost]
        public async Task<JsonResult> Save(int indexId, string[] criteriaValues)
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
                        UpdateDateInspektor = DateTime.Now,
                        IndexId = indexId,
                        CriteriaId = Convert.ToInt32(temp[0]),
                        ValueInspektor = Convert.ToDouble(temp[1]),
                        InspektorId = 1,
                        SchoolId = 1,
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
