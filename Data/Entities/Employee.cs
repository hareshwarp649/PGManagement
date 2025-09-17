using PropertyManage.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyManage.Data.Entities
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // ✅ Allows manual ID assignment
        public int Id { get; set; }

        public int? ManagerId { get; set; }
        public Employee? Manager { get; set; }
        public ICollection<Employee> Subordinates { get; set; } = new List<Employee>();


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


        [Required]
        public DateTime JoiningDate { get; set; }

        public EmployeeType EmployeeType { get; set; } = EmployeeType.FullTime;

        public string TotalExperience { get; set; } = string.Empty;



        // ✅ Designation
        public int? RoleId { get; set; }
        public virtual Role? Role { get; set; } = null!;

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

        public ICollection<EmployeeDocument> Documents { get; set; } = new List<EmployeeDocument>();
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
