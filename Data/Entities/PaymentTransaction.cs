using PropertyManage.Data.MasterEntities;
using PropertyManage.Domain.Enums;
using System.Transactions;

namespace PropertyManage.Data.Entities
{
    public class PaymentTransaction : BaseEntity
    {
        public Guid LeaseAgreementId { get; set; }
        public Guid PaymentModeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;// Success, Failed, Pending

        public LeaseAgreement LeaseAgreement { get; set; }
        public PaymentMode PaymentMode { get; set; }
    }
}
