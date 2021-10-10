using System.ComponentModel.DataAnnotations;

namespace BookingTickets.Models.ViewModels
{
    public class RoomVM
    {
        [Required]
        public string Name { get; set; }
        public int CinemaId { get; set; }
    }
}
