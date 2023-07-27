namespace Customer.Domain.Models
{
    public record CustomerViewModel
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
}
