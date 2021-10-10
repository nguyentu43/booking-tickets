using AutoMapper;
using BookingTickets.Data.Base;
using BookingTickets.Models;

namespace BookingTickets.Data
{
    public class ReservationSeatRepository : Repository<ReservationSeat>, IReservationSeatRepository
    {
        public ReservationSeatRepository(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}
