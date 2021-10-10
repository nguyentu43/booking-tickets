using BookingTickets.Data.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingTickets.Pages.Admin.Movie
{
    public class IndexModel : PageModel
    {
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private IUnitOfWork _unitOfWork { get; }
        public List<Models.Movie> Movies { get; set; }
        public async Task OnGetAsync()
        {
            Movies = await _unitOfWork.MovieRepository.GetAll().ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _unitOfWork.MovieRepository.Remove(id);
            await _unitOfWork.SaveChangeAsync();
            TempData["message"] = "Delete successful";
            return RedirectToPage();
        }
    }
}
