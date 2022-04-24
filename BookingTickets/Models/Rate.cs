using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTickets.Models
{
    public class Rate
    {
        [Key]
        public int Id { get; set; }
        public int ReservationId { get; set; }
        [ForeignKey("ReservationId")]
        public Reservation Reservation { get; set; }
        [Required]
        [Range(0, 5)]
        public int Value { get; set; }
        public string Content { get; set; }
        public DateTime RatedDate { get; set; }
    }
}
