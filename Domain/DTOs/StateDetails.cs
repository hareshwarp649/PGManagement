namespace PropertyManage.Domain.DTOs
{
    public class StateDetails
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid CountryId { get; set; }
        public string CountryName { get; set; }= string.Empty;
    }
}
