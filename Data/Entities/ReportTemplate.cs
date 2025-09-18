namespace PropertyManage.Data.Entities
{
    public class ReportTemplate : BaseEntity
    {
        public string TemplateName { get; set; }
        public string TemplateContent { get; set; } // HTML / JSON / XML
    }
}
