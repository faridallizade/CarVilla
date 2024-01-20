using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using CarVilla.Helpers;
using CarVilla.Models;
using CarVilla.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CarVilla.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(SignInManager<AppUser> signInManager,UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm vm)
        {
            AppUser user = new AppUser()
            {
                Name = vm.Name,
                Email = vm.Email,
                Surname = vm.Surname,
                UserName =  vm.Username
            };
            var res = await _userManager.CreateAsync(user,vm.Password);
            if (!res.Succeeded)
            {
                foreach (var error in res.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            await _userManager.AddToRoleAsync(user, UserRole.Admin.ToString());
            return RedirectToAction("Login","Account");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm vm)
        {
            var user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
            if(user == null)
            {
                user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
                if(user == null)
                {
                    ModelState.AddModelError("", "Username of Email Address is incorrect.");

                }
            }
            var res = await _signInManager.CheckPasswordSignInAsync(user, vm.Password,false);
            if (!res.Succeeded)
            {
                ModelState.AddModelError("", "Username of Email Address is incorrect.");

            }
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> CreateRole()
        {
            foreach (var role in Enum.GetValues(typeof(UserRole)))
            {
                var existsRole = await _roleManager.RoleExistsAsync(role.ToString());
                if (!existsRole)
                {
                    await _roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = role.ToString(),
                    });
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
