using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookingTickets.Data.Base;
using BookingTickets.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookingTickets.Pages.Admin.Genre
{
    public class EditModel : PageModel
    {
        public EditModel(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        private IUnitOfWork _unitOfWork { get; }
        private IMapper _mapper { get; }
        [BindProperty]
        public GenreVM Genre { get; set; }
        public async Task OnGetAsync(int? id = null)
        {
            Genre = _mapper.Map<Models.Genre, GenreVM>(await _unitOfWork.GenreRepository.DbSet.SingleOrDefaultAsync(g => g.Id == id));
        }

        public async Task<IActionResult> OnPostAsync(int? id = null)
        {
            if(ModelState.IsValid)
            {
                var editGenre = await _unitOfWork.GenreRepository.DbSet.AsNoTracking().SingleOrDefaultAsync(g => g.Id == id);
                var genre = _mapper.Map<GenreVM, Models.Genre>(Genre);
                if (editGenre == null)
                {
                    _unitOfWork.GenreRepository.Add(genre);
                    TempData["message"] = "Create successful";
                }
                else
                {
                    genre.Id = (int)id;
                    _unitOfWork.GenreRepository.Update(genre);
                    TempData["message"] = "Update successful";
                }
                await _unitOfWork.SaveChangeAsync();
                return RedirectToPage(new { id = genre.Id });
            }
            return Page();
        }
    }
}
