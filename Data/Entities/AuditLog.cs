namespace PropertyManage.Data.Entities
{
    public class AuditLog : BaseEntity
    {
        public string EntityName { get; set; }
        public Guid EntityId { get; set; }
        public string Action { get; set; } // Create, Update, Delete
        public string ActionBy { get; set; }
        public DateTime ActionDate { get; set; } = DateTime.UtcNow;
    }
}
