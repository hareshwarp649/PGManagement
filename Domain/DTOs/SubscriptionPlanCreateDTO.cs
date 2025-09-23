namespace PropertyManage.Domain.DTOs
{
    public class SubscriptionPlanCreateDTO
    {
        public string PlanName { get; set; }
        public decimal Price { get; set; }
        public string BillingCycle { get; set; }
        public int MaxProperties { get; set; }
        public int MaxBuildings { get; set; }
        public int MaxUnits { get; set; }
        public int MaxUsers { get; set; }
        public bool SupportIncluded { get; set; }
    }
}
