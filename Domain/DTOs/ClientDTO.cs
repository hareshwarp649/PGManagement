namespace PropertyManage.Domain.DTOs
{
    public class ClientDTO
    {
        public Guid Id { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public DateTime OnboardedAt { get; set; }
        public Guid? CreatedById { get; set; }    // optional
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
