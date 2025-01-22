using System.ComponentModel.DataAnnotations;

namespace BL.DTO
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Pasword is required")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password needs to be at least 8 characters long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [EmailAddress(ErrorMessage = "Wrong e-mail format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string Phone { get; set; }
    }
}
