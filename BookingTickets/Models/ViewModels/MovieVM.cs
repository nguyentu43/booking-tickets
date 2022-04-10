using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Required]
        public string Cover { get; set; }
        public string LandscapeCover { get; set; }
        public string Trailer { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<int> Genres { get; set; } = new List<int>();
    }
}
