using PropertyManage.Data.MasterEntities;

namespace PropertyManage.Data.Entities
{
    public class ExpenseTransaction : BaseEntity
    {
        public Guid PropertyId { get; set; }
        public Guid ExpenseCategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }

        public Property Property { get; set; }
        public ExpenseCategory ExpenseCategory { get; set; }
    }
}
