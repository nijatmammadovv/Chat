using Chat.Data_Access_Layer;
using Chat.Models;
using Chat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(AppDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Users = _context.Users.Where(u => u.UserName != User.Identity.Name).Include(u => u.Photo),
                CurrentUser =  _context.Users.SingleOrDefault(u => u.UserName == User.Identity.Name)
            };
            return View(homeVM);
        }
    }
}
