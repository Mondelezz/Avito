using System.ComponentModel.DataAnnotations;

namespace Avito.Models
{
    public class Persons
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? HashPassword { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<AdModels>? AdModels { get; set; }
        public List<FavoriteModel>? Favorites { get; set; }
        public string? PathFile { get; set; }
        public int PhotoId { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
