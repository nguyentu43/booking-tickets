using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingTickets.Data.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Models = BookingTickets.Models;

namespace BookingTickets.Pages.Admin.Cinema
{
   
    public class IndexModel : PageModel
    {
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private IUnitOfWork _unitOfWork { get; }
        public List<Models.Cinema> Cinemas { get; set; }
        public async Task OnGetAsync()
        {
            Cinemas = await _unitOfWork.CinemaRepository.GetAll().ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var deleteCinema = await _unitOfWork.CinemaRepository.DbSet.SingleOrDefaultAsync(g => g.Id == id);
            if(deleteCinema != null)
            {
                _unitOfWork.CinemaRepository.Remove(deleteCinema);
                await _unitOfWork.SaveChangeAsync();
                TempData["message"] = "Delete successful";
            }
            else
            {
                TempData["error_message"] = "Cinema not found";
            }
            return RedirectToPage();
        }
    }
}
