using System.ComponentModel.DataAnnotations;

namespace Bookshop_v5.Models.DTO
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
