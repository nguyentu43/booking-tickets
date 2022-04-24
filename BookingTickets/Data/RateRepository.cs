using AutoMapper;
using BookingTickets.Data.Base;
using BookingTickets.Models;

namespace BookingTickets.Data
{
    public class RateRepository : Repository<Rate>, IRateRepository
    {
        public RateRepository(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}
