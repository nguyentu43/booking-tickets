using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookingTickets.Constants;

namespace BookingTickets.Models
{
    [Table("Seats")]
    public class Seat
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Row { get; set; }
        [Required]
        public int Column { get; set; }
        [Required]
        public SeatType SeatType { get; set; }
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room Room { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}
