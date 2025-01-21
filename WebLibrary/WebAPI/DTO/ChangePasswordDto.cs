using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO
{
    public class ChangePasswordDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password needs to be atleast 8 charachters long")]
        public string NewPassword { get; set; }
    }
}
