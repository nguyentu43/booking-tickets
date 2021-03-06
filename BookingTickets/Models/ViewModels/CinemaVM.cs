using System.ComponentModel.DataAnnotations;

namespace BookingTickets.Models.ViewModels
{
    public class CinemaVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
