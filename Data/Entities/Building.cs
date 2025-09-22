namespace PropertyManage.Data.Entities
{
    public class Building : BaseEntity
    {
        public string BuildingName { get; set; }   // Tower A, Block 1
        public int Floors { get; set; }

        public Guid PropertyId { get; set; }
        public Propertiy Propertiy { get; set; }

        public ICollection<Unit> Units { get; set; }  // Flats under this building
    }
}
