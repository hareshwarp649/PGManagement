namespace PropertyManage.Data.Entities
{
    public class SecurityDeposit : BaseEntity
    {
        public Guid TenantId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidDate { get; set; }
        public string Status { get; set; } // Refunded, Active

        public Tenant Tenant { get; set; }
    }
}
