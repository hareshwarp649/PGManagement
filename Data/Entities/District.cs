namespace PropertyManage.Data.Entities
{
    public class District : BaseEntity
    {
        public string Name { get; set; }
        public Guid StateId { get; set; }
        public State State { get; set; }
    }
}
