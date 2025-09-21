namespace bca.api.DTOs
{
    public class UserDTO
    {

        public Guid Id { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public IEnumerable<string> Roles { get; set; }= new List<string>();
    }
}
