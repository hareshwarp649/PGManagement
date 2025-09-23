namespace PropertyManage.Domain.DTOs
{
    public class CreateClientSubscriptionDTO
    {
        public Guid ClientId { get; set; }
        public Guid SubscriptionPlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
