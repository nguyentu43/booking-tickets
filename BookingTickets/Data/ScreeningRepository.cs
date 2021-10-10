using AutoMapper;
using BookingTickets.Data.Base;
using BookingTickets.Models;

namespace BookingTickets.Data
{
    public class ScreeningRepository : Repository<Screening>, IScreeningRepository
    {
        public ScreeningRepository(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}
