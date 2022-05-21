using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ecom.Data.Services;
using ecom.Data.Cart;
using ecom.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using ecom.Models;
using ecom.Data.Static;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ecom.Data;
using Microsoft.EntityFrameworkCore;

namespace ecom.Controllers
{
    [Route("admin")]
    [Authorize(Roles = "Admin")]
     public class AdminUserController : Controller
     {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;

         public AdminUserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
         {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
         }
        [Route("users")]
         public async Task<IActionResult> Users()
         {
            var users =await _context.Users.ToListAsync();
            return View("index", users);
         }
     }
}