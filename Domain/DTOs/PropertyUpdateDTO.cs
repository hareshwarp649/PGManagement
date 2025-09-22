namespace PropertyManage.Domain.DTOs
{
    public class PropertyUpdateDTO
    {
        public string? PropertyName { get; set; }
        public Guid? PropertyTypeId { get; set; }
        public string? Address { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? StateId { get; set; }
        public Guid? DistrictId { get; set; }
        public int? FloorCount { get; set; }
        public int? TotalRooms { get; set; }
        public double? AreaInSqFt { get; set; }
    }
}
