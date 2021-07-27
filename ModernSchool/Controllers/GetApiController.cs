using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<JsonResult> GetRegions()
        {
            return Json(await db.Regions.OrderBy(x=>x.name_uz).ToListAsync());
        }

        public async Task<JsonResult> GetDistricts(int id = 0)
        {
            return Json(await db.Districts.Where(x => x.parent_id == id).ToListAsync());
        }

        public async Task<JsonResult> GetSchools(int id = 0)
        {
            return Json(await db.Schools.Where(x => x.DistrictId == id).ToListAsync());
        }

        public async Task<JsonResult> GetSchoolTypes()
        {
            return Json(await db.SchoolTypes.Where(x => x.IsSpec == false).ToListAsync());
        }
        public async Task<JsonResult> GetRoles()
        {
            return Json(await db.Roles.ToListAsync());
        }

        public async Task<JsonResult> GetIndexes(int id)
        {
            return Json(await db.Indexes.Where(x => x.Level < id).OrderBy(x => x.Level).ThenBy(x => x.Id).ToListAsync());
        }

        public async Task<JsonResult> GetIndexesByParent(int id)
        {
            return Json(await db.Indexes.Where(x => x.ParentId == id).ToListAsync());
        }
        public async Task<JsonResult> GetCriteriasByIndexId(int id)
        {
            return Json(await db.Indexes.Include(x=>x.Criterias).Where(x => x.Id == id).ToListAsync());
        }
    }
}
