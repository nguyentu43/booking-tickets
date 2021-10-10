using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingTickets.Models
{
    public class Cinema
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public List<Room> Rooms { get; set; } = new List<Room>();
    }
}
