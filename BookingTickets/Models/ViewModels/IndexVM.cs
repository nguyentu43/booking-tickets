using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTickets.Models.ViewModels
{
    public class IndexVM
    {
        public List<Movie> CarouselMovies { get; set; }
        public List<Movie> NewestMovies { get; set; }
        public List<Movie> MostViewedMovies { get; set; }
        public List<Genre> Genres { get; set; }

    }
}
