using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTickets.Models.ViewModels
{
    public class SearchFormVM
    {
        public int SelectedGenre { get; set; }
        public string Keyword { get; set; }
        public int PageTotal { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
