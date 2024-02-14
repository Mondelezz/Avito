namespace Avito.Models
{
    public class FilterModel
    {
        public string? Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? CategoryModelId { get; set; }
    }
}
