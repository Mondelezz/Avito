using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Avito.Models
{
    public class Photos
    {
        public int Id { get; set; }
        [JsonIgnore]
        public List<AdModelPhoto>? AdModels { get; set; }
        public string? PathFile { get; set; }
        public string? FileName { get; set; }

    }
}
