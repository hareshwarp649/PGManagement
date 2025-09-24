using PropertyManage.Data.MasterEntities;

namespace PropertyManage.Data.Entities
{
    public class ExpenseTransaction : BaseEntity
    {
        public Guid UnitId { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime Date { get; set; }

        // Navigation
        public Unit Unit { get; set; }
    }
}
