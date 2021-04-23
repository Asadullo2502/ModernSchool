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
    [Authorize]
    public class AdminController : Controller
    {
        private DataContext db;

        public AdminController(DataContext context)
        {
            db = context;
        }

        public IActionResult Dashboard()
        {
            return RedirectToAction("Departments");
        }

        #region Departments
        public async Task<IActionResult> Regions()
        {
            return View(await db.Departments.Include(x=>x.Departments).Where(x => x.Type == 2).ToListAsync());
        }

        public IActionResult CreateRegion()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateRegion(Department department)
        {
            try
            {
                db.Departments.Add(department);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Regions");
        }

        public IActionResult EditRegion(int id = 0)
        {
            return View(db.Departments.FirstOrDefault(x => x.Id == id));
        }
        [HttpPost]
        public IActionResult EditRegion(Department department)
        {
            try
            {
                db.Entry(department).State = EntityState.Modified;
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
                var department = db.Departments.FirstOrDefault(x => x.Id == id);
                db.Departments.Remove(department);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Regions");
        }

        public async Task<IActionResult> Departments(int? RegionId = 0)
        {
            if (RegionId != 0 && RegionId != null)
            {
                var data = await db.Departments.Include(x => x.Departments).Where(x => x.ParentId == RegionId).ToListAsync();
                ViewBag.region = RegionId;
                return View(data);
            }
            else
            {
                var data = await db.Departments.Include(x => x.Departments).Where(x => x.Type == 3).ToListAsync();
                ViewBag.region = 0;
                return View(data);
            }
        }

        public IActionResult CreateDepartment()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateDepartment(Department department)
        {
            try
            {
                department.Type = db.Departments.FirstOrDefault(x => x.Id == department.ParentId).Type + 1;
                db.Departments.Add(department);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Departments");
        }

        public IActionResult EditDepartment(int id = 0)
        {
            return View(db.Departments.FirstOrDefault(x => x.Id == id));
        }
        [HttpPost]
        public IActionResult EditDepartment(Department department)
        {
            try
            {
                department.Type = db.Departments.FirstOrDefault(x => x.Id == department.ParentId).Type + 1;
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Departments");
        }

        [HttpPost]
        public IActionResult DeleteDepartment(int id)
        {
            try
            {
                var department = db.Departments.FirstOrDefault(x => x.Id == id);
                db.Departments.Remove(department);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Departments");
        }
        #endregion

        #region Schools
        public async Task<IActionResult> Schools()
        {
            return View(await db.Schools.Include(x => x.Department.Departments).ToListAsync());
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
            return View(db.Schools.Include(x => x.Department.Departments).FirstOrDefault(x => x.Id == id));
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
    }
}
