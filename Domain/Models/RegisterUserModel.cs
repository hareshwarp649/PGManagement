using PropertyManage.Domain.Enums;

namespace bca.api.Models
{
    public class RegisterUserModel
    {
        public string Username { get; set; } 
        public string Password { get; set; } 
        public string RoleName { get; set; }  // SUPERADMIN / ADMIN
        public UserType UserType { get; set; }
    }
}
