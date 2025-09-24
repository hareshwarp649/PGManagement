using PropertyManage.Domain.Enums;

namespace PropertyManage.Data.Entities
{
    public class Tenant : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public string Accupation { get; set; }
        public TenantType TenantType { get; set; } = TenantType.Family;
        public DateTime MoveInDate { get; set; }
        public DateTime? MoveOutDate { get; set; }

        public Guid UnitId { get; set; }
        public Unit Unit { get; set; }
        public TenantAgreement TenantAgreement { get; set; }  
    }
}
