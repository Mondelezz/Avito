using System.ComponentModel.DataAnnotations;

namespace Avito.Models
{
    public class UpdatePersonPassword
    {
        public string CurrentPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Confirm password")]
        public string? ConfirmPassword { get; set; }
    }
}
