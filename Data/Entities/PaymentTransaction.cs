using PropertyManage.Data.MasterEntities;
using PropertyManage.Domain.Enums;
using System.Transactions;

namespace PropertyManage.Data.Entities
{
    public class PaymentTransaction : BaseEntity
    {
        public Guid RentInvoiceId { get; set; }
        public Guid PaymentModeId { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime Date { get; set; }

        // Navigation
        public RentInvoice RentInvoice { get; set; }
        public PaymentMode PaymentMode { get; set; }
    }
}
