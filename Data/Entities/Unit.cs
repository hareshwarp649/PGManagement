using MathNet.Numerics.RootFinding;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyManage.Data.Entities
{
    public class Unit : BaseEntity
    {
        public string UnitNumber { get; set; }
        public string UnitType { get; set; }
        public int Capacity { get; set; } // Beds for PG/Hostel
        public decimal Rent { get; set; }

        public Guid PropertyId { get; set; }
        public Propertiy Propertiy { get; set; }

        public Guid? BuildingId { get; set; } // Apartment only
        public Building Building { get; set; }

        public ICollection<Tenant> Tenants { get; set; }

        [NotMapped]
        public bool IsOccupied => Tenants != null && Tenants.Any(t => t.MoveOutDate == null || t.MoveOutDate > DateTime.UtcNow);

        [NotMapped]
        public int AvailableBeds => Capacity - (Tenants?.Count(t => t.MoveOutDate == null || t.MoveOutDate > DateTime.UtcNow) ?? 0);
    }
}
