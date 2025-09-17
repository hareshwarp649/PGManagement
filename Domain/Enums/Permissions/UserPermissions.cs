using System.ComponentModel.DataAnnotations;

namespace bca.api.Enums.Permissions
{
    public enum UserPermissions
    {
        [Display(Name = "Can Manage Admin")]
        CanManageAdmin,

        [Display(Name = "Can Manage SPOC")]
        CanManageSPOC,

        [Display(Name = "Can Manage Employee")]
        CanManageEmployee,

        [Display(Name = "Can View Exception Logs")]
        CanViewExceptionLogs
    }
}
