namespace PropertyManage.Domain.DTOs
{
    public class StateInput
    {
        public required string Name { get; set; } 
        public Guid CountryId { get; set; }
    }
}
