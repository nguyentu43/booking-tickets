using System.Collections.Generic;

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
