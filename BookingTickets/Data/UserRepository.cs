using AutoMapper;
using BookingTickets.Data.Base;
using BookingTickets.Models;

namespace BookingTickets.Data
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}
