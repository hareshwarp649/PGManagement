using PropertyManage.Data.Entities;

namespace PropertyManage.Data.MasterEntities
{
    public class ExpenseCategory : BaseEntity
    {
        public string CategoryName { get; set; } // Maintenance, Utility, Repair
    }
}
