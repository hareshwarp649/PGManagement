using NPOI.Util;
using PropertyManage.Domain.Enums;

namespace PropertyManage.Data.Entities
{
    public class Propertiy : BaseEntity
    {
        public Guid ClientId { get; set; }
        public Client Client { get; set; }
        public string PropertyName { get; set; }
        public Guid PropertyTypeId { get; set; }
        public PropertyType PropertyType { get; set; } // Apartment, PG, Hostel, Commercial

        public string Address { get; set; }
        public int PinCode { get; set; }
        public Guid DistrictId { get; set; }       
        public District District { get; set; }
        public int TotalUnits { get; set; } 
        public ICollection<Unit> Units { get; set; }       
    }
}
