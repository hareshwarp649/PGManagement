using bca.api.DTOs;
using bca.api.Models;
using PropertyManage.Domain.Models;

namespace PropertyManage.ServiceInfra.IServices
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> LoginAsync(LoginDTO dto, string deviceInfo);
        Task<AuthResponseDTO?> RefreshAsync(string refreshToken, string deviceInfo);
        Task LogoutAsync(string refreshToken);
    }
}
