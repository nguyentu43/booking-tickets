using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingTickets.Models.ViewModels
{
    public class ReservationFormVM
    {
        [Required]
        [JsonRequired]
        public string Name { get; set; }
        [Required]
        [JsonRequired]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [JsonRequired]
        [RegularExpression(@"\d{10,15}")]
        public string Phone { get; set; }
        [Required]
        [JsonRequired]
        public DateTime CardDate { get; set; }
        [Required]
        [JsonRequired]
        [RegularExpression(@"^\d{4} \d{4} \d{4} \d{4}$")]
        public string CardNumber { get; set; }
        [Required]
        [JsonRequired]
        [RegularExpression(@"\d{3,5}")]
        public string Cvc { get; set; }
        [Required]
        [JsonRequired]
        public int ScreeningId { get; set; }
        [Required]
        [JsonRequired]
        public string CaptchaId { get; set; }
        [Required]
        [JsonRequired]
        public string Captcha { get; set; }

        [Required]
        [JsonRequired]
        public List<int> Seats { get; set; }
    }
}
