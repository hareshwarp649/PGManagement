using bca.api.Enums;
using PropertyManage.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace bca.api.DTOs
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string ReferencePersonName { get; set; } = string.Empty;
        public string ReferencePersonContactNumber { get; set; } = string.Empty;
        public Gender Gender { get; set; }

        // ✅ Contact Information
        public int? StateId { get; set; }
        public string? StateName { get; set; } = string.Empty;
        public string ResidentialAddress { get; set; } = string.Empty;
        public string PermanentAddress { get; set; } = string.Empty;
        public string PersonalEmail { get; set; } = string.Empty;
        public string OfficialEmail { get; set; } = string.Empty;
        public string PersonalPhoneNumber { get; set; } = string.Empty;
        public string EmergencyContactNumber { get; set; } = string.Empty;
        public string AadharNumber { get; set; } = string.Empty;
        public string PANNumber { get; set; } = string.Empty;

        // ✅ Employment Information
        public int? RoleId { get; set; }
        public string? RoleName { get; set; } = string.Empty;
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; } = string.Empty;
        public int? TeamId { get; set; }
        public string? TeamName { get; set; } = string.Empty;
        public DateTime JoiningDate { get; set; }
        public EmployeeType EmployeeType { get; set; } = EmployeeType.FullTime;
        public string TotalExperience { get; set; } = string.Empty;
        public string OurCompanyExperience => GetExperience(JoiningDate);

        public int? ManagerId { get; set; }
        public string ManagerName { get; set; } = string.Empty;

        // ✅ Educational & Professional Information
        [MaxLength(100)]
        public string HighestEducation { get; set; } = string.Empty;
        public string PreviousEmploymentHistory { get; set; } = string.Empty;

        // ✅ Banking Details
        public string BankName { get; set; } = string.Empty;
        public string SavingAccountNumber { get; set; } = string.Empty;
        public string IFSCCode { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;


        // ✅ Method to Calculate Experience
        private string GetExperience(DateTime joiningDate)
        {
            var years = DateTime.Now.Year - joiningDate.Year;
            var months = DateTime.Now.Month - joiningDate.Month;
            if (months < 0)
            {
                years--;
                months += 12;
            }
            return $"{years} Years {months} Months";
        }
    }
}
