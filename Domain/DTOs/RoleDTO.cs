namespace bca.api.DTOs
{
    public class RoleDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public List<string> Permissions { get; set; } = new();
    }
}
