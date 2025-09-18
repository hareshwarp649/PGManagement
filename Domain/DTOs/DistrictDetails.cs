namespace PropertyManage.Domain.DTOs
{
    public class DistrictDetails
    {
        public Guid Id { get; set; }
        public required string Name { get; set; } 
        public Guid StateId { get; set; }
        public string StateName { get; set; } = string.Empty;
    }
}
