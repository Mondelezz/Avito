namespace Avito.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public int TargetId { get; set; }
        public string TargetName { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public Persons Persons { get; set; }

    }
}
