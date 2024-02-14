namespace Avito.Models
{
    public class PhotoInput
    {
        public int AdModelId { get; set; }
        public IFormFileCollection? File { get; set; }
    }
}
