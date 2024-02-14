using System.ComponentModel.DataAnnotations;

namespace Avito.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int ParentId { get; set; }
    }
}
