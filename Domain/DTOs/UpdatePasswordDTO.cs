namespace bca.api.DTOs
{
    public class UpdatePasswordDTO
    {
        public required string UserId { get; set; }
        public required string NewPassword { get; set; }
    }
}
