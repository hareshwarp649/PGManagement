namespace PropertyManage.Domain.DTOs
{
    public class UnitDTO
    {
        public Guid Id { get; set; }
        public string UnitNumber { get; set; }
        public string UnitType { get; set; }
        public int Capacity { get; set; }
        public decimal Rent { get; set; }
        public bool IsOccupied { get; set; }
        public int AvailableBeds { get; set; }
        public Guid PropertyId { get; set; }
        public Guid? BuildingId { get; set; }
    }
}
