using PropertyManage.Domain.Enums;

namespace PropertyManage.Data.Entities
{
    public class Tenant : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime MoveInDate { get; set; }
        public DateTime? MoveOutDate { get; set; }

        public Guid UnitId { get; set; }
        public Unit Unit { get; set; }

        // Optional: Tenant type
        public TenantType TenantType { get; set; } = TenantType.Permanent;

        // Relationships
        public ICollection<LeaseAgreement> LeaseAgreements { get; set; } = new List<LeaseAgreement>();
        public ICollection<PaymentTransaction> Payments { get; set; } = new List<PaymentTransaction>();
        public SecurityDeposit SecurityDeposit { get; set; }
    }
}
