
using bca.api.DTOs;
using PropertyManage.Data.Entities;

namespace bca.api.Services
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync();
        Task<PermissionDTO?> GetPermissionByIdAsync(int id);
        Task<PermissionDTO> AddPermissionAsync(Permission permission);
        Task<PermissionDTO?> UpdatePermissionAsync(int id, Permission permission);
        Task<bool> DeletePermissionAsync(int id);
        Task<IEnumerable<PermissionDTO>> SearchAndSortPermissionsAsync(string? name, string? category, string? description, string? sortBy, string? sortOrder, int pageNumber, int pageSize);
        Task<bool> HasPermissionAsync(string userName, string permissionName);
    }
}
