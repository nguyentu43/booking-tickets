using BookingTickets.Models;
using System.Linq;

namespace BookingTickets.Data.Base
{
    public interface IRoomRepository : IRepository<Room>
    {
        IQueryable<Room> GetAllWithCinema();
    }
}
