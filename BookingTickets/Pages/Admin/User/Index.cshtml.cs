using BookingTickets.Data.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingTickets.Pages.Admin.User
{
    public class IndexModel : PageModel
    {
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private IUnitOfWork _unitOfWork { get; }
        public List<Models.ApplicationUser> Users { get; set; }
        public async Task OnGetAsync()
        {
            Users = await _unitOfWork.UserRepository.GetAll().ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var deleteUser = await _unitOfWork.UserRepository.DbSet.SingleOrDefaultAsync(u => u.Id == id);
            if (deleteUser != null)
            {
                _unitOfWork.UserRepository.Remove(deleteUser);
                await _unitOfWork.SaveChangeAsync();
                TempData["message"] = "Delete successful";
            }
            else
            {
                TempData["error_message"] = "User not found";
            }
            return RedirectToPage();
        }
    }
}
