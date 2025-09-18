using PropertyManage.Data.Entities;

namespace PropertyManage.Data.MasterEntities
{
    public class PaymentMode : BaseEntity
    {
        public string ModeName { get; set; } // UPI, Card, NetBanking, Cash
    }
}
