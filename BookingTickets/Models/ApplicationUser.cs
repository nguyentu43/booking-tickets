using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingTickets.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(25, MinimumLength = 6)]
        public string Name { get; set; }

        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
