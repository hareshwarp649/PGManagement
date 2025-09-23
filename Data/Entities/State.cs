namespace PropertyManage.Data.Entities
{
    public class State 
    {
        public Guid Id { get; set; }
        public string StateName { get; set; }
        public Guid CountryId { get; set; }
        public Country Country { get; set; }
        public ICollection<District> Districts { get; set; }
    }
}
