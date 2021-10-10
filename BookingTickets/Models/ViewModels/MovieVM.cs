using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTickets.Models.ViewModels
{
    public class MovieVM
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Director { get; set; }
        [Required]
        public string Cast { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Language { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public string Rated { get; set; }
        public string Cover { get; set; }
        [Display(Name ="Cover File")]
        public IFormFile CoverFile { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<int> Genres { get; set; } = new List<int>();
    }
}
