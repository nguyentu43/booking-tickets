using BookingTickets.Data.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTickets.Pages.Admin.Movie.Screening
{
    public class IndexModel : PageModel
    {
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private IUnitOfWork _unitOfWork { get; }
        public List<IGrouping<int, Models.Screening>> ScreeningGroups { get; set; }
        public async Task OnGetAsync(int movieId)
        {
            var screenings = await _unitOfWork.ScreeningRepository.DbSet
                .Where(sc => sc.MovieId == movieId)
                .Include(sc => sc.Movie)
                .Include(sc => sc.Room)
                .ThenInclude(r => r.Cinema)
                .ToListAsync();
            ScreeningGroups = screenings.GroupBy(sc => sc.Room.CinemaId).ToList();
        }
        public async Task<IActionResult> OnGetRoomAsync(int cinemaId)
        {
            var rooms = await _unitOfWork.RoomRepository.DbSet.Where(r => r.CinemaId == cinemaId).ToListAsync();
            return new JsonResult(rooms);
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var screening = await _unitOfWork.ScreeningRepository.DbSet.SingleOrDefaultAsync(sc => sc.Id == id);
            if (screening != null)
            {
                _unitOfWork.ScreeningRepository.Remove(screening);
                await _unitOfWork.SaveChangeAsync();
                TempData["message"] = "Delete successful";
            }
            else
            {
                throw new Exception("Screening not found");
            }

            return RedirectToPage();
        }
    }
}
