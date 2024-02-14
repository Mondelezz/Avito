using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSwag.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Avito.Models
{
    public class AdModelInput
    {

        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string? Location { get; set; }
        public int CategoryModelId { get; set; }
    }
}
