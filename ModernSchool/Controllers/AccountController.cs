using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModernSchool.DB;
using ModernSchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ModernSchool.Controllers
{
    public class AccountController : Controller
    {
        private DataContext db;
        public AccountController(DataContext dataContext)
        {
            db = dataContext;
        }
        public IActionResult Login()
        {
            try
            {
                int role = Convert.ToInt32(User.FindFirst(x => x.Type == "RoleId").Value);

                if (role == 1)
                    return RedirectToAction("RatedSchools", "Admin");

                if (role == 2)
                    return RedirectToAction("Index", "Home");

                if (role == 5)
                    return RedirectToAction("Profile", "SchoolProfile");

                if (role == 6)
                    return RedirectToAction("Orders", "Rater");
            }
            catch { }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string login, string password)
        {
            if (ModelState.IsValid)
            {
                var user = await db.Users.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);

                if (user != null)
                {
                    await Authenticate(user);

                    if (user.RoleId == 1)
                        return RedirectToAction("RatedSchools", "Admin");

                    if (user.RoleId == 2)
                        return RedirectToAction("Index", "Home");

                    if (user.RoleId == 5)
                        return RedirectToAction("Profile", "SchoolProfile");

                    if (user.RoleId == 6)
                        return RedirectToAction("Orders", "Rater");
                }
                ViewBag.error = "Login yoki parol noto'gri";
            }
            return View();
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.RoleId.ToString()),
                new Claim("RegionId", user.RegionId.ToString()),
                new Claim("DistrictId", user.DistrictId.ToString()),
                new Claim("SchoolId", user.SchoolId.ToString()),
                new Claim("RoleId", user.RoleId.ToString())
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
