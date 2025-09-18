namespace PropertyManage.Data.Entities
{
    public class ClientSubscription : BaseEntity
    {
        public Guid ClientId { get; set; }
        public Guid SubscriptionPlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        public Client Client { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; }
    }
}
