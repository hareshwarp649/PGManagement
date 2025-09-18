namespace PropertyManage.Data.Entities
{
    public class Owner : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public ICollection<Property> Properties { get; set; }
    }
}
