using Chat.Data_Access_Layer;
using Chat.Models;
using Chat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager { get; }

        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,
            IWebHostEnvironment env,
            AppDbContext context,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _env = env;
            _context = context;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            AppUser user = _context.Users.SingleOrDefault(u => u.UserName == login.UserName);
            var result= await _signInManager.PasswordSignInAsync(login.UserName, login.Password, true, true);
            if (!result.Succeeded)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            AppUser user = new AppUser
            {
                FullName= register.Fullname,
                UserName=register.Username
            };
            await _userManager.CreateAsync(user, register.Password);
            string fileName = register.Username + register.Image.FileName;
            using (FileStream fs = new FileStream(Path.Combine(_env.WebRootPath,"",fileName),FileMode.Create))
            {
                register.Image.CopyTo(fs);
            }
            UserPhoto userPhoto = new UserPhoto
            {
                AppUser = user,
                ImageUrl = fileName
            };
            await _context.UserPhotos.AddAsync(userPhoto);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
