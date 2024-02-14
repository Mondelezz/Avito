using System.ComponentModel.DataAnnotations;

namespace Avito.Models
{
    public class RegistrationCode
    {
        [MaxLength(4)]
        public string Code { get; set; }
    }
}
