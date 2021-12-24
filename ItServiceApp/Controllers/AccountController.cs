using ItServiceApp.Models;
using ItServiceApp.Models.Identity;
using ItServiceApp.Services;
using ItServiceApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItServiceApp.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEmailSender _emailSender;
        
        public AccountController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            RoleManager<ApplicationRole> roleManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            CheckRoles();
        }

        private void CheckRoles()
        {
            foreach (var roleName in RoleModels.Roles)
            {
                if (!_roleManager.RoleExistsAsync(roleName).Result)
                {
                    var result = _roleManager.CreateAsync(new ApplicationRole()
                    {
                        Name = roleName
                    }).Result;
                }
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public  async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                ModelState.AddModelError(nameof(model.UserName), "Bu kullanıcı adı daha önce alınmıştır.");
                return View(model);
            }
            user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                ModelState.AddModelError(nameof(model.Email), "Bu email adresiyle daha önce kayıt olumuştur.");
                return View(model);
            }
            user = new ApplicationUser()
            {
                Email = model.Email,
                Name = model.Name,
                UserName = model.UserName,
                Surname = model.Surname
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                //kullanıcıya rol atama
                var count = _userManager.Users.Count();
                result = await _userManager.AddToRoleAsync(user, count == 1 ? RoleModels.Admin : RoleModels.User);//eğer kullanıcı sayısı 1 ise admin,değilse düz user rolü atıyor.
                //email onay maili
                await _emailSender.SendAsync(new EmailMessage()
                {
                    Contacts = new string[] { user.Email },
                    Subject = $"{user.UserName} - kullanıcı kayıt oldu.",
                    Body = $"{user.Name} {user.Surname} isimli kullanıcı {DateTime.Now:g} itibari ile siteye kayıt olmuştur."

                });
                //login sayfasına yönlendirme
                return RedirectToAction("Login", "Account");//home controllerın index sayfasına yönlendir
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bir hata oluştu");
                return View(model);
            }
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                await _emailSender.SendAsync(new EmailMessage()
                {
                    Contacts = new string[] { user.Email },
                    Subject = $"{user.UserName} - kullanıcı giriş yaptı.",
                    Body = $"{user.Name} {user.Surname} isimli kullanıcı {DateTime.Now:g} itibari ile siteye giriş yapmıştır."

                });
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı");
                return View(model);
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
