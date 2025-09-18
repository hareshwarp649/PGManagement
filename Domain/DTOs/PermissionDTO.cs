namespace bca.api.DTOs
{
    public class PermissionDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; } // Optional for Permission Name
        public string? Description { get; set; } = string.Empty;
        public string? Category { get; set; } = string.Empty;
    }
}
