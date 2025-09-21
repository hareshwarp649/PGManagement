
using bca.api.DTOs;
using PropertyManage.Data.Entities;

namespace bca.api.Services
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync();
        Task<PermissionDTO?> GetPermissionByIdAsync(Guid id);
        Task<PermissionDTO> AddPermissionAsync(PermissionDTO permission);
        Task<PermissionDTO?> UpdatePermissionAsync(Guid id, PermissionDTO permission);
        Task<bool> DeletePermissionAsync(Guid id);

       

    }
}
