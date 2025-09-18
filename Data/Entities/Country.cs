namespace PropertyManage.Data.Entities
{
    public class Country:BaseEntity
    {
        public string CountryName { get; set; }
        public string Code { get; set; } = string.Empty;
        public ICollection<State> States { get; set; }
    }
}
