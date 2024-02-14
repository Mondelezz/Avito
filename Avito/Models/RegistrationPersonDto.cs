using System.ComponentModel.DataAnnotations;

namespace Avito.Models
{
    public class RegistrationPersonDto
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Не соотвествует формату. Пример: " +
            "\" \"ivan223@yandex.ru/ivan223@mail.ru/ivan223@gmail.com\"")]
        public string? Email { get; set; }
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
