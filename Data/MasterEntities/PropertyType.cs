using PropertyManage.Data.Entities;

namespace PropertyManage.Data.MasterEntities
{
    public class PropertyType: BaseEntity
    {
       public string TypeName { get; set; } // Apartment, Villa, Studio, Office
         public ICollection<Propertiy> Properties { get; set; }
        
    }
}
