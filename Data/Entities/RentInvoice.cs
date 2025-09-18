namespace PropertyManage.Data.Entities
{
    public class RentInvoice : BaseEntity
    {
        public Guid LeaseAgreementId { get; set; }
        public decimal Amount { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }

        public LeaseAgreement LeaseAgreement { get; set; }
    }
}
