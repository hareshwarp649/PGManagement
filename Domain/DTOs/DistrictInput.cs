namespace PropertyManage.Domain.DTOs
{
    public class DistrictInput
    {
        public required string Name { get; set; } 
        public Guid StateId { get; set; }
    }
}
