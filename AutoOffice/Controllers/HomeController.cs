using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoOffice.Models;
using Microsoft.AspNetCore.Identity;
using AutoOffice.Data;

namespace AutoOffice.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext db;

        public HomeController(
          UserManager<ApplicationUser> userManager,
          ApplicationDbContext injectedContext)
        {
            _userManager = userManager;
            db = injectedContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> HumanResourceManage()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var username = user.UserName;
            if (username != "root@root.com")
            {
                throw new ApplicationException($"You don't have this power, user {username}.");
            }

            var model = new HumanResourceManageModel
            {
                HumanManages = db.HumanManages.ToArray()
            };

            return View(model);
        }
        
        public async Task<IActionResult> HumanResourceManageSetJobTo(string userName, string job)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var username = user.UserName;
            if (username != "root@root.com")
            {
                throw new ApplicationException($"You don't have this power, user {username}.");
            }

            HumanManage humanManageToUpdate = db.HumanManages.First(p => p.UserName == userName);
            humanManageToUpdate.Job = job;
            db.SaveChanges();

            return View();
        }
        
    }
}
