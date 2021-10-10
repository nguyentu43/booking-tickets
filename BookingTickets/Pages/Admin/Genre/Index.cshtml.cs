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

namespace BookingTickets.Pages.Admin.Genre
{
   
    public class IndexModel : PageModel
    {
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private IUnitOfWork _unitOfWork { get; }
        public List<Models.Genre> Genres { get; set; }
        public async Task OnGetAsync()
        {
            Genres = await _unitOfWork.GenreRepository.GetAll().ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var deleteGenre = await _unitOfWork.GenreRepository.DbSet.SingleOrDefaultAsync(g => g.Id == id);
            if(deleteGenre != null)
            {
                _unitOfWork.GenreRepository.Remove(deleteGenre);
                await _unitOfWork.SaveChangeAsync();
                TempData["message"] = "Delete successful";
            }
            else
            {
                TempData["error_message"] = "Genre not found";
            }
            return RedirectToPage();
        }
    }
}
