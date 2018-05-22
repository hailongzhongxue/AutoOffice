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

        // MARK: - 人事管理 HumanResourceManage
        public async Task<IActionResult> HumanResourceManage()
        {
            // 验证是否登陆
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var username = user.UserName;
            // 验证是否登为管理用户
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
        
        [HttpPost]
        public async Task<IActionResult> HumanResourceManageSet(string email, string name, string department, string job)
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

            HumanManage humanManageToUpdate = db.HumanManages.First(p => p.Email == email);
            humanManageToUpdate.Name = name;
            humanManageToUpdate.Job = job;
            humanManageToUpdate.Department = department;
            db.SaveChanges();

            return View();
        }

        // MARK: - 站内短信 Message 
        public async Task<IActionResult> Message()
        {
            // 验证是否登陆
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new MessageModel
            {
                Messages = db.Messages.ToArray()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MessageSend(string toEmail, string text)
        {
            // 验证是否登陆
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            Message message = new Message();
            message.FromEmail = user.UserName;
            message.ToEmail = toEmail;
            message.Text = text;
            message.Time = DateTime.Now;
            db.Messages.Add(message);
            db.SaveChanges();

            return View();
        }
    }
}
