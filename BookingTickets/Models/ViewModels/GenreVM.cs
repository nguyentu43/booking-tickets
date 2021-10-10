using System.ComponentModel.DataAnnotations;

namespace BookingTickets.Models.ViewModels
{
    public class GenreVM
    {
        [Required]
        public string Name { get; set; }
    }
}
