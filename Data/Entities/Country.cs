namespace PropertyManage.Data.Entities
{
    public class Country
    {
        public Guid Id { get; set; }
        public string CountryName { get; set; }
        public string Code { get; set; } = string.Empty;
        public ICollection<State> States { get; set; }
    }
}
