namespace PropertyManage.Data.Entities
{
    public class ApplicationUserRefreshToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public bool Revoked { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? DeviceInfo { get; set; }
        public string? ReplacedByToken { get; set; }
    }
}
