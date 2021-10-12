using AutoMapper;
using BookingTickets.Models;
using BookingTickets.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace BookingTickets.Pages.Admin.User
{
    public class AddModel : PageModel
    {
        public AddModel(IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }
        private UserManager<ApplicationUser> _userManager { get; }
        private IMapper _mapper { get; }
        [BindProperty]
        public AddUserVM UserVM { get; set; }
        public async Task<IActionResult> OnPostAsync(string id = null)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<ApplicationUser>(UserVM);
                var passwordHasher = new PasswordHasher<ApplicationUser>();
                var result = await _userManager.CreateAsync(user, passwordHasher.HashPassword(user, UserVM.Password));
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return Page();
                }
                await _userManager.AddToRoleAsync(user, UserVM.Role.ToString());
                return RedirectToPage("Update", new { id = user.Id });
            }
            return Page();
        }
    }
}
