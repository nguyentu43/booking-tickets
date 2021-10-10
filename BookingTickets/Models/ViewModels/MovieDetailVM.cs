using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTickets.Models.ViewModels
{
    public class MovieDetailVM
    {
        public Movie Movie { get; set; }
        public List<Room> Rooms { get; set; }
    }
}
