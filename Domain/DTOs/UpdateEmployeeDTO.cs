using bca.api.Enums;
using PropertyManage.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace bca.api.DTOs
{
    public class UpdateEmployeeDTO
    {
        // ✅ Personal Information
        [Required, MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string MiddleName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(100)]
        public string ReferencePersonName { get; set; } = string.Empty;

        [MaxLength(15)]
        public string ReferencePersonContactNumber { get; set; } = string.Empty;

        [Required]
        public Gender Gender { get; set; }

        // ✅ Contact Information
        public int StateId { get; set; }

        [Required]
        public string ResidentialAddress { get; set; } = string.Empty;

        [Required]
        public string PermanentAddress { get; set; } = string.Empty;

        [MaxLength(100)]
        public string PersonalEmail { get; set; } = string.Empty;

        [MaxLength(100)]
        public string OfficialEmail { get; set; } = string.Empty;

        [MaxLength(15)]
        public string PersonalPhoneNumber { get; set; } = string.Empty;

        [MaxLength(15)]
        public string EmergencyContactNumber { get; set; } = string.Empty;

        [Required]
        public string AadharNumber { get; set; } = string.Empty;

        [Required]
        public string PANNumber { get; set; } = string.Empty;

        // ✅ Employment Information        
        public int? RoleId { get; set; }
        public int? DepartmentId { get; set; }
        public int? TeamId { get; set; }

        [Required]
        public DateTime JoiningDate { get; set; }

        public EmployeeType EmployeeType { get; set; } = EmployeeType.FullTime;

        public string TotalExperience { get; set; } = string.Empty;
        public int? ManagerId { get; set; }

        // ✅ Educational & Professional Information
        [MaxLength(100)]
        public string HighestEducation { get; set; } = string.Empty;

        public string PreviousEmploymentHistory { get; set; } = string.Empty;

        // ✅ Banking Details
        public string BankName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string SavingAccountNumber { get; set; } = string.Empty;

        [MaxLength(15)]
        public string IFSCCode { get; set; } = string.Empty;
    }
}
