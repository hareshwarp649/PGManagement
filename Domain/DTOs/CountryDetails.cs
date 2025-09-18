namespace PropertyManage.Domain.DTOs
{
    public class CountryDetails
    {
        public Guid Id { get; set; }
        public required string Name { get; set; } 
        public required string Code { get; set; }
    }
}
