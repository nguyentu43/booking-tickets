using BookingTickets.Data.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingTickets.Pages.Admin.Reservation
{
    public class IndexModel : PageModel
    {
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private IUnitOfWork _unitOfWork { get; }
        public List<Models.Reservation> Reservations { get; set; }
        public async Task OnGetAsync()
        {
            Reservations = await _unitOfWork.ReservationRepository.DbSet
                .Include(r => r.Seats)
                .ThenInclude(s => s.Room).ThenInclude(r => r.Cinema)
                .Include(r => r.Screening)
                .ThenInclude(sc => sc.Movie)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var reservation = await _unitOfWork.ReservationRepository.DbSet.SingleOrDefaultAsync(r => r.Id == id);
            _unitOfWork.ReservationRepository.Remove(reservation);
            await _unitOfWork.SaveChangeAsync();
            TempData["message"] = "Delete successful";
            return RedirectToPage();
        }
    }
}
