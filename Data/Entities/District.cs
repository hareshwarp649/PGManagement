namespace PropertyManage.Data.Entities
{
    public class District 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid StateId { get; set; }
        public State State { get; set; }
        public ICollection<Propertiy> Properties { get; set; }
    }
}
