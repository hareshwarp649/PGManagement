namespace PropertyManage.Domain.Models
{
    public class AuthResponseDTO
    {
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public DateTime AccessTokenExpiresAt { get; set; }
        public bool MustChangePassword { get; set; } = false;
        public string Message { get; set; } = "";
    }
}
