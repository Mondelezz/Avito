using System.ComponentModel.DataAnnotations;

namespace Avito.Models
{
    public class CodeDb
    {
        public int Id { get; set; }
        public int Code { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
