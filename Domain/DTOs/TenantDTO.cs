using PropertyManage.Domain.Enums;

namespace PropertyManage.Domain.DTOs
{
    public class TenantDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime MoveInDate { get; set; }
        public DateTime? MoveOutDate { get; set; }
        public Guid UnitId { get; set; }
        public TenantType TenantType { get; set; }
    }
}
