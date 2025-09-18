namespace PropertyManage.Data.Entities
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public Guid? TenantId { get; set; }
        public Guid? OwnerId { get; set; }
    }
}
