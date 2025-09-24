namespace PropertyManage.Data.Entities
{
    public class Client : BaseEntity
    {
        public string ClientName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string ContactPerson { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public DateTime OnboardedAt { get; set; } = DateTime.UtcNow;

        // Relationships
        public ICollection<Propertiy> Properties { get; set; }           
        public ICollection<ClientSubscription> Subscriptions { get; set; }
        public ICollection<PaymentTransaction> PaymentTransactions { get; set; }
    }
}
