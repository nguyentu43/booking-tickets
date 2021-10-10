using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookingTickets.Models.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Display(Name ="Remember Me?")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
