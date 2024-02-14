using System.ComponentModel.DataAnnotations;

namespace Avito.Models
{
    public class AuthPerson
    {
        [Required]
        [EmailAddress(ErrorMessage ="Email не соответствует формату. пример: user@example.com")]
        public string? Email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string? Password { get; set; }

    }
}
