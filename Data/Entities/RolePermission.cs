using System.Text.Json.Serialization;

namespace PropertyManage.Data.Entities
{
    public class RolePermission
    {
        public int RoleId { get; set; }
        [JsonIgnore]
        public Role Role { get; set; } = null!;
        public int PermissionId { get; set; }
        [JsonIgnore]
        public Permission Permission { get; set; } = null!;
    }
}
