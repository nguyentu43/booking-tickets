using System.Collections.Generic;

namespace BookingTickets.Models.ViewModels
{
    public class MovieDetailVM
    {
        public Movie Movie { get; set; }
        public List<Room> Rooms { get; set; }
    }
}
