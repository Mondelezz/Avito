using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Avito.Models
{
    public class AdModels
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int NumberOfViews { get; set; }
        public decimal Price { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public string? NameCategory { get; set; }
        public string? Location { get; set; }
        public int Likes { get; set; }
        public bool IsFavorite { get; set; }
        [ForeignKey("PersonId")]
        public int PersonId { get; set; }
        public Persons? Person { get; set; }

        [ForeignKey("CategoryModelId")]
        public int CategoryModelId { get; set; }
        public CategoryModel? Category { get; set; }
        public string? PathDirectory { get; set; }
        [JsonIgnore]
        public List<AdModelPhoto>? Photos { get; set; }

    }
}
