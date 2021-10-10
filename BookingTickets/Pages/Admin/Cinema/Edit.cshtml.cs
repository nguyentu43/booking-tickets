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

namespace BookingTickets.Pages.Admin.Cinema
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
        public CinemaVM Cinema { get; set; }
        public async Task OnGetAsync(int? id = null)
        {
            Cinema = _mapper.Map<CinemaVM>(await _unitOfWork.CinemaRepository.DbSet.SingleOrDefaultAsync(g => g.Id == id));
        }
        public async Task<IActionResult> OnPostAsync(int? id = null)
        {
            if(ModelState.IsValid)
            {
                var editCinema = await _unitOfWork.CinemaRepository.DbSet.AsNoTracking().SingleOrDefaultAsync(g => g.Id == id);
                var cinema = _mapper.Map<CinemaVM, Models.Cinema>(Cinema);
                if (editCinema == null)
                {
                    _unitOfWork.CinemaRepository.Add(cinema);
                    TempData["message"] = "Create successful";
                }
                else
                {
                    cinema.Id = (int)id;
                    _unitOfWork.CinemaRepository.Update(cinema);
                    TempData["message"] = "Update successful";
                }
                await _unitOfWork.SaveChangeAsync();
                return RedirectToPage(new { id = cinema.Id });
            }
            return Page();
        }
    }
}
