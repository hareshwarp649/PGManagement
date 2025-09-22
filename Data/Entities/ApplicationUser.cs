using Microsoft.AspNetCore.Identity;
using NPOI.SS.Formula.Functions;
using PropertyManage.Domain.Enums;

namespace PropertyManage.Data.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {      
        public Guid? EntityId { get; set; }
        public UserType UserType { get; set; }
        public bool IsDeleted { get; set; }=false;

        public string FullName { get; set; } = string.Empty;
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<ApplicationUserRefreshToken> RefreshTokens { get; set; } = new List<ApplicationUserRefreshToken>();
       
    }
   
}
