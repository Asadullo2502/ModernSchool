using Microsoft.AspNetCore.Mvc;
using ModernSchool.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Controllers
{
    public class GetApiController : Controller
    {
        private DataContext db;

        public GetApiController(DataContext context)
        {
            db = context;
        }
        public JsonResult GetRegions()
        {
            return Json(db.Departments.Where(x => x.Type == 2));
        }

        public JsonResult GetDistricts(int id = 0)
        {
            return Json(db.Departments.Where(x => x.ParentId == id));
        }

        public JsonResult GetSchools(int id = 0)
        {
            return Json(db.Schools.Where(x => x.DepartmentId == id));
        }

        public JsonResult GetSchoolTypes()
        {
            return Json(db.SchoolTypes.Where(x => x.IsSpec == false));
        }
        public JsonResult GetRoles()
        {
            return Json(db.Roles);
        }

        public JsonResult GetIndexes(int id)
        {
            return Json(db.Indexes.Where(x => x.Level < id).OrderBy(x => x.Level).ThenBy(x => x.Id));
        }
    }
}
