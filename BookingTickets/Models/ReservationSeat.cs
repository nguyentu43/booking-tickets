using BookingTickets.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        public string SeatName {get;set;}
    }
}
