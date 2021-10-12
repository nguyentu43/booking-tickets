using System.ComponentModel.DataAnnotations;

namespace BookingTickets.Models.ViewModels
{
    public class UpdateUserVM
    {
        [Required, EnumDataType(typeof(Constants.Role))]
        public Constants.Role Role { get; set; }
        [Required]
        [StringLength(25)]
        public string Name { get; set; }
        [Required]
        [StringLength(25, MinimumLength = 5)]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [StringLength(25, MinimumLength = 5)]
        public string Password { get; set; }
    }
}
