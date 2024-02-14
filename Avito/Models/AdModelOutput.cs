using System.ComponentModel.DataAnnotations;

namespace Avito.Models
{
    public class AdModelOutput
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
        public int PersonId { get; set; }
        public int CategoryModelId { get; set; }
        public List<AdModelPhotoOutput>? Photos { get; set; }

    }
}
