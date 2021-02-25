using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; } //in DTOs properties  not case sensitive!

        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; }
    }
}