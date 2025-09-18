namespace PropertyManage.Data.Entities
{
    public class Attachment : BaseEntity
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public Guid RelatedEntityId { get; set; }
        public string RelatedEntityType { get; set; } // LeaseAgreement, Unit, Invoice etc.
    }
}
