using PropertyManage.Data.MasterEntities;
using PropertyManage.Domain.Enums;

namespace PropertyManage.Data.Entities
{
    public class TenantAgreement : BaseEntity
    {
        public Guid TenantId { get; set; }
        public DateTime AgreementDate { get; set; }
        public int ValidPeriodMonths { get; set; }
        public AgreementType AgreementType { get; set; } 
        public DateTime ExpiryDate { get; set; }
        public decimal SecurityDeposit { get; set; }
        public decimal RentAmount { get; set; }
        public decimal MaintenanceAmount { get; set; }
        public RentPlan RentPlan { get; set; }
        public Tenant Tenant { get; set; }
        public ICollection<RentInvoice> RentInvoices { get; set; }

    }
}
