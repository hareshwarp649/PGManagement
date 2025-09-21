﻿using System.Text.Json.Serialization;

namespace PropertyManage.Data.Entities
{
    public class RolePermission
    {
        public Guid RoleId { get; set; }
        public ApplicationRole Role { get; set; } = null!;

        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; } = null!;
    }
}
