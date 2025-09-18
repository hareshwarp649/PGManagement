using PropertyManage.Data.Entities;

namespace PropertyManage.Data.MasterEntities
{
    public class RentPlan : BaseEntity
    {
        public string PlanName { get; set; } // Monthly, Quarterly, Yearly
        public int DurationInMonths { get; set; }
    }
}
