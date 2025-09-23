namespace PropertyManage.Domain.DTOs
{
    public class ClientSubscriptionDTO
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Guid SubscriptionPlanId { get; set; }
        public string ClientName { get; set; }
        public string PlanName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
