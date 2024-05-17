using Database.Entities.Concretes;
using Maxim_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Maxim_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }
            User user = new User()
            {
                UserName = registerVM.Email,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Email = registerVM.Email,
            };
            var result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            User user;
            if (loginVM.Email.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(loginVM.Email);
            }
            else
            {
                user = await _userManager.FindByNameAsync(loginVM.Email);
            }
            if (user == null)
            {
                ModelState.AddModelError("", "Username Or Email Incorrect!");
                return View();
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginVM.Password, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Try Again Later!");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username Or Email Incorrext");
                return View();
            }
            await _signInManager.SignInAsync(user, loginVM.RememberMe);
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");

        }



    }
}
