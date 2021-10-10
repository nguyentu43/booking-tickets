using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTickets.Models.ViewModels
{
    public class ReservationFormVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"\d{10,15}")]
        public string Phone { get; set; }
        [Required]
        public int ScreeningId { get; set; }
        [Required]
        public List<int> Seats { get; set; }
    }
}
