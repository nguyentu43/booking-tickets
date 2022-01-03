using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public DateTime CardDate { get; set; }
        [Required]
        [RegularExpression(@"\d{16}")]
        public string CardNumber { get; set; }
        [Required]
        [RegularExpression(@"\d{3,5}")]
        public string Cvc { get; set; }
        [Required]
        public int ScreeningId { get; set; }
        [Required]
        public List<int> Seats { get; set; }
    }
}
