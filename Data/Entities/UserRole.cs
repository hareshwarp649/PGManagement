namespace PropertyManage.Data.Entities
{
    public class UserRole
    {
        public required string UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public required int RoleId { get; set; }
        public Role Role { get; set; } = null!;
    }
}
