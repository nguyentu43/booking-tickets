using BookingTickets.Constants;
using System.ComponentModel.DataAnnotations;

namespace BookingTickets.Models.ViewModels
{
    public class SeatVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int Row { get; set; }
        [Required]
        public int Column { get; set; }
        [Required]
        public SeatType SeatType { get; set; }
        [Required]
        public int RoomId { get; set; }
    }
}
