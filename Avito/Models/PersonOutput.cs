namespace Avito.Models
{
    public class PersonOutput
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int PhotoId { get; set; }
        public List<ReviewOutput> Reviews { get; set; }
    }
}
