namespace PropertyManage.Data.Entities
{
    public class MaintenanceRequest : BaseEntity
    {
        public Guid UnitId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // Pending, Completed
        public DateTime RequestDate { get; set; }

        public Unit Unit { get; set; }
    }
}
