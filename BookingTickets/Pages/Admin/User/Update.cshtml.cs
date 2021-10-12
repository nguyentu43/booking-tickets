using AutoMapper;
using BookingTickets.Models;
using BookingTickets.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace BookingTickets.Pages.Admin.User
{
    public class UpdateModel : PageModel
    {
        public UpdateModel(IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }
        private UserManager<ApplicationUser> _userManager { get; }
        private IMapper _mapper { get; }
        [BindProperty]
        public UpdateUserVM UserVM { get; set; }
        public async Task OnGetAsync(string id = null)
        {
            var user = await _userManager.FindByIdAsync(id);
            UserVM = _mapper.Map<UpdateUserVM>(user);
            var roles = await _userManager.GetRolesAsync(user);
            UserVM.Role = roles[0] == "Admin" ? Constants.Role.Admin : Constants.Role.Customer;
        }
        public async Task<IActionResult> OnPostAsync(string id = null)
        {
            if (ModelState.IsValid)
            {
                var editUser = await _userManager.FindByIdAsync(id);
                editUser.Name = UserVM.Name;
                var passwordHasher = new PasswordHasher<ApplicationUser>();
                if (UserVM.Password != null)
                {
                    editUser.PasswordHash = passwordHasher.HashPassword(editUser, UserVM.Password);
                }
                await _userManager.UpdateAsync(editUser);
                await _userManager.AddToRoleAsync(editUser, UserVM.Role.ToString());
                return RedirectToPage(new { id = editUser.Id });
            }
            return Page();
        }
    }
}
