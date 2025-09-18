using PropertyManage.Data.MasterEntities;

namespace PropertyManage.Data.Entities
{
    public class LeaseAgreement : BaseEntity
    {
        public Guid TenantId { get; set; }
        public Guid UnitId { get; set; }
        public Guid RentPlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Tenant Tenant { get; set; }
        public Unit Unit { get; set; }
        public RentPlan RentPlan { get; set; }

    }
}
