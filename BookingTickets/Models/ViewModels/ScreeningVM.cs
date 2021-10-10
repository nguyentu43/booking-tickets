using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
