using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModernSchool.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModernSchool.Models;
using Microsoft.AspNetCore.Authorization;

namespace ModernSchool.Controllers
{
    [Authorize(Roles = "1")]
    public class AdminController : Controller
    {
        private DataContext db;

        public AdminController(DataContext context)
        {
            db = context;
        }

        public IActionResult Dashboard()
        {
            return RedirectToAction("Districts");
        }

        #region Departments
        public async Task<IActionResult> Regions()
        {
            return View(await db.Regions.ToListAsync());
        }

        public IActionResult CreateRegion()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateRegion(Region region)
        {
            try
            {
                db.Regions.Add(region);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Regions");
        }

        public IActionResult EditRegion(int id = 0)
        {
            return View(db.Regions.FirstOrDefault(x => x.id == id));
        }
        [HttpPost]
        public IActionResult EditRegion(Region region)
        {
            try
            {
                db.Entry(region).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Regions");
        }

        [HttpPost]
        public IActionResult DeleteRegion(int id)
        {
            try
            {
                var region = db.Regions.FirstOrDefault(x => x.id == id);
                db.Regions.Remove(region);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Regions");
        }

        public async Task<IActionResult> Districts(int? RegionId = 0)
        {
            if (RegionId != 0 && RegionId != null)
            {
                var data = await db.Districts.Include(x => x.Region).Where(x => x.parent_id == RegionId).ToListAsync();
                ViewBag.region = RegionId;
                return View(data);
            }
            else
            {
                var data = await db.Districts.Include(x => x.Region).ToListAsync();
                ViewBag.region = 0;
                return View(data);
            }
        }

        public IActionResult CreateDistrict()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateDistrict(District district)
        {
            try
            {
                db.Districts.Add(district);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Districts");
        }

        public IActionResult EditDistrict(int id = 0)
        {
            return View(db.Districts.FirstOrDefault(x => x.id == id));
        }
        [HttpPost]
        public IActionResult EditDistrict(District district)
        {
            try
            {
                db.Entry(district).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Districts");
        }

        [HttpPost]
        public IActionResult DeleteDistrict(int id)
        {
            try
            {
                var distict = db.Districts.FirstOrDefault(x => x.id == id);
                db.Districts.Remove(distict);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Districts");
        }
        #endregion

        #region Schools
        public async Task<IActionResult> Schools()
        {
            return View(await db.Schools.Include(x => x.District).Include(x => x.Region).ToListAsync());
        }

        public IActionResult CreateSchool()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateSchool(School school)
        {
            try
            {
                db.Schools.Add(school);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Schools");
        }

        public IActionResult EditSchool(int id = 0)
        {
            return View(db.Schools.Include(x => x.District).FirstOrDefault(x => x.Id == id));
        }
        [HttpPost]
        public IActionResult EditSchool(School school)
        {
            try
            {
                db.Entry(school).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Schools");
        }

        [HttpPost]
        public IActionResult DeleteSchool(int id)
        {
            try
            {
                var school = db.Schools.FirstOrDefault(x => x.Id == id);
                db.Schools.Remove(school);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Schools");
        }
        #endregion

        #region Users
        public async Task<IActionResult> Users()
        {
            return View(await db.Users.Include(x => x.Role).Include(x => x.Region).Include(x => x.District).Include(x => x.School).ToListAsync());
        }

        public IActionResult CreateUser()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            try
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Users");
        }

        public IActionResult EditUser(int id = 0)
        {
            return View(db.Users.FirstOrDefault(x => x.Id == id));
        }
        [HttpPost]
        public IActionResult EditUser(User user)
        {
            try
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Users");
        }

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var user = db.Users.FirstOrDefault(x => x.Id == id);
                db.Users.Remove(user);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Users");
        }
        #endregion

        #region Indexes
        public async Task<IActionResult> Indexes(int? id = 0)
        {
            //if (id != null && id != 0)
            //{
            //    return View(await db.Indexes.Include(x => x.Parent).Where(x => x.ParentId == id).ToListAsync());
            //}
            {
                return View(await db.Indexes.ToListAsync());
            }
        }

        public IActionResult CreateIndex()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateIndex(Models.Index index)
        {
            try
            {
                index.Level = db.Indexes.FirstOrDefault(x => x.Id == index.ParentId).Level + 1;
                db.Indexes.Add(index);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Indexes");
        }

        public IActionResult EditIndex(int id = 0)
        {
            return View(db.Indexes.FirstOrDefault(x => x.Id == id));
        }
        [HttpPost]
        public IActionResult EditIndex(Models.Index index)
        {
            try
            {
                index.Level = db.Indexes.FirstOrDefault(x => x.Id == index.ParentId).Level + 1;
                db.Entry(index).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Indexes");
        }

        [HttpPost]
        public IActionResult DeleteIndex(int id)
        {
            try
            {
                var index = db.Indexes.FirstOrDefault(x => x.Id == id);
                db.Indexes.Remove(index);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Indexes");
        }
        #endregion

        #region Criterias
        public async Task<IActionResult> Criterias()
        {
            return View(await db.Criterias.Include(x => x.Index).ToListAsync());
        }

        public IActionResult CreateCriteria()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCriteria(Criteria criteria)
        {
            try
            {
                db.Criterias.Add(criteria);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Criterias");
        }

        public IActionResult EditCriteria(int id = 0)
        {
            return View(db.Criterias.FirstOrDefault(x => x.Id == id));
        }
        [HttpPost]
        public IActionResult EditCriteria(Criteria criteria)
        {
            try
            {
                db.Entry(criteria).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Criterias");
        }

        [HttpPost]
        public IActionResult DeleteCriteria(int id)
        {
            try
            {
                var criteria = db.Criterias.FirstOrDefault(x => x.Id == id);
                db.Criterias.Remove(criteria);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Criterias");
        }
        #endregion

        #region Menu
        public async Task<IActionResult> Menu()
        {
            return View(await db.Menus.ToListAsync());
        }

        public IActionResult CreateMenu()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateMenu(Menu menu)
        {
            try
            {
                db.Menus.Add(menu);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Menu");
        }

        public IActionResult EditMenu(int id = 0)
        {
            return View(db.Menus.FirstOrDefault(x => x.id == id));
        }
        [HttpPost]
        public IActionResult EditMenu(Menu menu)
        {
            try
            {
                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Menu");
        }

        [HttpPost]
        public IActionResult DeleteMenu(int id)
        {
            try
            {
                var menu = db.Menus.FirstOrDefault(x => x.id == id);
                db.Menus.Remove(menu);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Menu");
        }
        #endregion
    }
}
