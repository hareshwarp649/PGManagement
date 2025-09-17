using System.ComponentModel;

namespace PropertyManage.Domain.Enums
{
    public enum DocumentType
    {
        [Description("Photograph")]
        Photo,

        [Description("Signature")]
        Signature,

        [Description("Aadhar Card")]
        AadharCard,

        [Description("PAN Card")]
        PANCard,

        [Description("Education Certificate")]
        EducationCertificate,

        [Description("Payment Proof")]
        PaymentProof,

    }
}
