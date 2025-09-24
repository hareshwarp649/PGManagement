namespace PropertyManage.Domain.DTOs
{
    public class UnitDTO
    {
        public Guid Id { get; set; }
        public string UnitNumber { get; set; }
        public string UnitType { get; set; }
        public int Capacity { get; set; }
        public int FloorNumber { get; set; }
        public double AreaInSqFt { get; set; }
        public bool IsOccupied { get; set; }
        public Guid PropertyId { get; set; }
    }
}
