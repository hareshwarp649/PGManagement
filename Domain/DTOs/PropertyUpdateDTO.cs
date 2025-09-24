namespace PropertyManage.Domain.DTOs
{
    public class PropertyUpdateDTO
    {
        public string? PropertyName { get; set; }
        public Guid? PropertyTypeId { get; set; }
        public string? Address { get; set; }
        public int? PinCode { get; set; }
        public Guid? DistrictId { get; set; }
        public int? TotalUnits { get; set; }
    }
}
