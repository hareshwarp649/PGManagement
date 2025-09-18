using PropertyManage.Data.MasterEntities;

namespace PropertyManage.Data.Entities
{
    public class UtilityBill : BaseEntity
    {
        public Guid UnitId { get; set; }
        public Guid UtilityTypeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime BillDate { get; set; }

        public Unit Unit { get; set; }
        public UtilityType UtilityType { get; set; }
    }
}
