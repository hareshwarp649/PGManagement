namespace PropertyManage.Data.Entities
{
    public class RentInvoice : BaseEntity
    {
        public Guid TenantAgreementId { get; set; }
        public decimal RentAmount { get; set; }
        public decimal MaintenanceAmount { get; set; }
        public decimal ElectricityBill { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal RemainingAmount { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TotalDays { get; set; }

        // Navigation
        public TenantAgreement TenantAgreement { get; set; }
        public ICollection<PaymentTransaction> PaymentTransactions { get; set; }
    }
}
