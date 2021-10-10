﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingTickets.Models.ViewModels
{
    public class DetailsReservation
    {
        [Required]
        public Reservation Reservation { get; set; }
        [Required]
        public List<ReservationSeat> ReservationSeats { get; set; }
    }
}
