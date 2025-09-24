namespace PropertyManage.Domain.DTOs
{
    public class UnitCreateDTO
    {
        public string UnitNumber { get; set; }
        public string UnitType { get; set; }
        public int Capacity { get; set; }
        public int FloorNumber { get; set; }
        public double AreaInSqFt { get; set; }
        public Guid PropertyId { get; set; }
        public Guid? BuildingId { get; set; }

    }
}
