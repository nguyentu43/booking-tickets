using System.Collections.Generic;

namespace BookingTickets.Models.ViewModels
{
    public class IndexHomeVM
    {
        public List<Movie> Movies { get; set; }
        public List<Genre> Genres { get; set; }
        public SearchFormVM Form { get; set; }
    }
}
