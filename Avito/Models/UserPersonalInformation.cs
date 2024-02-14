using System.ComponentModel.DataAnnotations;

namespace Avito.Models
{
    public class UserPersonalInformation
    {
        public string? Name { get; set; }

        [EmailAddress(ErrorMessage = "Не соотвествует формату. Пример: \"ivan223@yandex.ru/ivan223@mail.ru/ivan223@gmail.com\"")]
        public string? Email { get; set; }
        
        public IFormFile? Photo { get; set; }

    }
}
