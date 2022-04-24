using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTickets.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public List<Seat> Seats { get; set; }
        public int ScreeningId { get; set; }
        [ForeignKey("ScreeningId")]
        public Screening Screening { get; set; }
        [RegularExpression(@"\d{10, 15}"), Required]
        public string Phone { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public ApplicationUser Customer { get; set; }
        [Required]
        public double Price { get; set; } = 0;
        public DateTime ReservationDate { get; set; }
        public Rate Rate { get; set; }
    }
}
