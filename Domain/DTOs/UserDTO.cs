namespace bca.api.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public int? BankId { get; set; }
        public string? BankName { get; set; }
    }
}
