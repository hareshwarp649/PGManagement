namespace PropertyManage.Data.Entities
{
    public class Permission
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;     // unique, e.g. "Products.Create"
        public string? Description { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
