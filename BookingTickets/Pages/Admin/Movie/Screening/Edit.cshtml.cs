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
using Models = BookingTickets.Models;
using BookingTickets.Helper;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace BookingTickets.Pages.Admin.Movie.Screening
{
    public class EditModel : PageModel
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IWebHostEnvironment _enviroment;
        public EditModel(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _enviroment = environment;
        }
        [BindProperty]
        public ScreeningVM Screening { get; set; }
        public List<Models.Room> Rooms { get; set; }
        public async Task OnGetAsync(int? id = null)
        {
            Screening = _mapper.Map<ScreeningVM>(await _unitOfWork.ScreeningRepository.DbSet.SingleOrDefaultAsync(sc => sc.Id == id));
            Rooms = await _unitOfWork.RoomRepository.DbSet.Include(r => r.Cinema).OrderBy(r => r.CinemaId).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(int? id = null)
        {
            Rooms = await _unitOfWork.RoomRepository.DbSet.Include(r => r.Cinema).OrderBy(r => r.CinemaId).ToListAsync();
            if (ModelState.IsValid)
            {
                var sc = _mapper.Map<Models.Screening>(Screening);
                sc.MovieId = Convert.ToInt32(RouteData.Values["MovieId"]);
                if (id == null)
                {
                    _unitOfWork.ScreeningRepository.Add(sc);
                    await _unitOfWork.SaveChangeAsync();
                    TempData["message"] = "Create successful";
                }
                else
                {
                    sc.Id = (int)id;
                    _unitOfWork.ScreeningRepository.Update(sc);
                    await _unitOfWork.SaveChangeAsync();
                }
                return RedirectToPage(new { id = sc.Id });
            }
            return Page();
        }
    }
}
