using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Avito.Models
{
    public class FavoriteModel
    {
        [Key]
        public int Id { get; set; }
        [Key]
        public int UserId { get; set; }
        public ICollection<Persons?> User { get; set; } = new List<Persons?>();
        [Key]
        [ForeignKey("AdModelId")]
        public int AdModelId { get; set; }
        public AdModels? AdModel { get; set; }

    }
}
