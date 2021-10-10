using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTickets.Models.ViewModels
{
    public class IndexHomeVM
    {
        public List<Movie> Movies { get; set; }
        public List<Genre> Genres { get; set; }
        public SearchFormVM Form { get; set; }
    }
}
