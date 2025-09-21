using Microsoft.AspNetCore.Identity;

namespace PropertyManage.Data.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string? Description { get; set; } = string.Empty;

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
