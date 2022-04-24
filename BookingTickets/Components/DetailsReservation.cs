using BookingTickets.Data.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ViewModels = BookingTickets.Models.ViewModels;

namespace BookingTickets.Components
{
    public class DetailsReservation : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public DetailsReservation(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var reservation = await _unitOfWork.ReservationRepository.DbSet
                .Include(r => r.Rate).Include(r => r.Screening)
                .ThenInclude(r => r.Movie)
                .SingleOrDefaultAsync(r => r.Id == id);
            var reservationSeats = await _unitOfWork.ReservationSeatRepository
                .DbSet.Where(rs => rs.ReservationId == reservation.Id)
                .Include(rs => rs.Seat)
                .ThenInclude(s => s.Room).ThenInclude(r => r.Cinema).ToListAsync();
            return View(new ViewModels.ReservationDetailVM
            {
                Reservation = reservation,
                ReservationSeats = reservationSeats
            });
        }
    }
}
