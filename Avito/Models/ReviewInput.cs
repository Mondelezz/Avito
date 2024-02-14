using Microsoft.Build.Framework;

namespace Avito.Models
{
    public class ReviewInput
    {
        [Required]
        public int TargetId { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
