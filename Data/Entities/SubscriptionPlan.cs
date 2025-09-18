namespace PropertyManage.Data.Entities
{
    public class SubscriptionPlan : BaseEntity
    {
        public string PlanName { get; set; }        // Basic, Standard, Premium
        public decimal Price { get; set; }
        public string BillingCycle { get; set; }    // Monthly / Yearly
        public int MaxProperties { get; set; }
        public int MaxBuildings { get; set; }
        public int MaxUnits { get; set; }
        public int MaxUsers { get; set; }
        public bool SupportIncluded { get; set; }

        public ICollection<ClientSubscription> ClientSubscriptions { get; set; }
    }
}
