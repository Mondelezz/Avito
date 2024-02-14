namespace Avito.Models
{
    public class AdModelPhotoInput
    {
        public AdModels? AdModel { get; set; }
        public int AdModelId { get; set; }

        public Photos? Photo { get; set; }
        public int PhotoId { get; set; }
    }
}
