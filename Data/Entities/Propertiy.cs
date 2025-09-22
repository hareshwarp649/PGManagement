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
        public Guid CountryId { get; set; }
        public Guid StateId { get; set; }
        public Guid DistrictId { get; set; }
        public Country Country { get; set; }
        public State State { get; set; }
        public District District { get; set; }

        public int FloorCount { get; set; } 
        public int TotalRooms { get; set; } 
        public double AreaInSqFt { get; set; }

        // Relationships
        public ICollection<Building> Buildings { get; set; }   // Only for Apartment type
        public ICollection<Unit> Units { get; set; }           // PG, Hostel, Commercial
    }
}
