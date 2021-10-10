using BookingTickets.Constants;
using BookingTickets.Filters;
using BookingTickets.Models;
using BookingTickets.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookingTickets.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<ApplicationUser> _signInManager { get; }
        public AuthController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HasLogin]
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginVM { ReturnUrl = returnUrl });
        }

        [HasLogin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (model.ReturnUrl != null)
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Username, password not match");
            }
            return View(model);
        }

        [HasLogin]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HasLogin]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Name = model.Name,
                    UserName = model.UserName,
                    Email = model.Email
                };

                var createdResult = await _signInManager.UserManager.CreateAsync(user, model.Password);
                if (!createdResult.Succeeded)
                {
                    foreach (var error in createdResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    await _signInManager.UserManager.AddToRoleAsync(user, Role.Customer.ToString());
                    var loginResult = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

                    if (loginResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View();
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
