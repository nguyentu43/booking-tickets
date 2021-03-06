using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingTickets.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
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
        public string Trailer { get; set; }
        public string LandscapeCover { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public List<Genre> Genres { get; set; } = new List<Genre>();
        public List<Screening> Screenings { get; set; } = new List<Screening>();

    }
}
