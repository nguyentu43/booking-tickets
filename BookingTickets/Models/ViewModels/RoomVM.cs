using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTickets.Models.ViewModels
{
    public class RoomVM
    {
        [Required]
        public string Name { get; set; }
        public int CinemaId { get; set; }
    }
}
