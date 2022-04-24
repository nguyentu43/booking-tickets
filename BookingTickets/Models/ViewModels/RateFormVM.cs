using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BookingTickets.Models.ViewModels
{
    public class RateFormVM
    {
        [Required]
        [JsonRequired]
        public int ReservationId { get; set; }
        [Required]
        [JsonRequired]
        [Range(0, 5)]
        public int Value { get; set; }
        public string Content { get; set; }
    }
}
