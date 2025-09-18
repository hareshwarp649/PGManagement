namespace PropertyManage.Data.Entities
{
    public class PenaltyTransaction : BaseEntity
    {
        public Guid TenantId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public DateTime PenaltyDate { get; set; }

        public Tenant Tenant { get; set; }
    }
}
