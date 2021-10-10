using System;
using System.ComponentModel.DataAnnotations;

namespace BookingTickets.Models.ViewModels
{
    public class ScreeningVM
    {
        [Required]
        public int RoomId { get; set; }
        [Required]
        public int MovieId { get; set; }
        [Required]
        public DateTime ScreeningStart { get; set; }
        [Required]
        public string Format { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
