using System.ComponentModel.DataAnnotations;

namespace BookingTickets.Models
{
    public class ReservationSeat
    {
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }
        public int SeatId { get; set; }
        public Seat Seat { get; set; }
        [Required]
        public double Price { get; set; } = 0;
        [Required]
        public string SeatName { get; set; }
    }
}
