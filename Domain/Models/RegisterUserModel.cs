using PropertyManage.Domain.Enums;

namespace bca.api.Models
{
    public class RegisterUserModel
    {
        public string Username { get; set; } 
        public string Password { get; set; } 
        public string RoleName { get; set; }  // SUPERADMIN / ADMIN
        public string FullName { get; set; }
        public UserType UserType { get; set; }
        public Guid? ClientId { get; set; }

    }
}
