using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingTickets.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
