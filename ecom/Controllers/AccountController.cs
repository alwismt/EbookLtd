using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ecom.Models;
using ecom.Data;
using ecom.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace ecom.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _appDbContext;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("login")]
        public IActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else{
                var response = new LoginVm();
                return View("Login", response);
            }


            
        }
        [HttpPost]
        public async Task<IActionResult> LoginCheck(LoginVm loginVm)
        {
            if(!ModelState.IsValid) return View("Login", loginVm);

            var user = await _userManager.FindByEmailAsync(loginVm.EmailAddress);
            if(user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVm.Password);
                if(passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVm.Password, false, false);
                    if(result.Succeeded)
                    {
                        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                        //var isAdmin = User.IsInRole("Admin");
                        if(isAdmin)
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        
                    }
                }
                TempData["Error"] ="Wrong credentials. Please, try again!";
                return View("Login", loginVm);
            }
            TempData["Error"] ="Wrong credentials. Please, try again!";
            return View("Login", loginVm);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("logout")]
        [Authorize]
        public async Task<IActionResult> GetLogout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}