using NPOI.Util;
using PropertyManage.Domain.Enums;

namespace PropertyManage.Data.Entities
{
    public class Property : BaseEntity
    {
        public string PropertyName { get; set; }
        public PropertyType PropertyType { get; set; } // Apartment, PG, Hostel, Commercial

        public string Address { get; set; }
        public Guid StateId { get; set; }
        public Guid DistrictId { get; set; }
        public Country Country { get; set; }
        public State State { get; set; }
        public District District { get; set; }

        public int floorCount { get; set; } 
        public int TotalRooms { get; set; } 
        public double AreaInSqFt { get; set; }
        public Guid OwnerId { get; set; }
        public Owner Owner { get; set; }

        // Relationships
        public ICollection<Building> Buildings { get; set; }   // Only for Apartment type
        public ICollection<Unit> Units { get; set; }           // PG, Hostel, Commercial
    }
}
