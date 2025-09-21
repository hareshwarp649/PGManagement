namespace PropertyManage.Domain.DTOs
{
    public class UserCreateDTO
    {
        public string FullName { get; set; } 
        public string Email { get; set; } 
        public string Password { get; set; } 
        public Guid? EntityId { get; set; }
        public IEnumerable<string>? Roles { get; set; }
    }
}
