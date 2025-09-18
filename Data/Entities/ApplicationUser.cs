using Microsoft.AspNetCore.Identity;
using NPOI.SS.Formula.Functions;
using PropertyManage.Domain.Enums;

namespace PropertyManage.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
      
        public int? EntityId { get; set; }
        public UserType UserType { get; set; }
        public bool IsDeleted { get; set; }

        //public int? BankId { get; set; }
        //public virtual Bank? Bank { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
       
    }

   
}
