using System.ComponentModel.DataAnnotations;

namespace bca.api.Enums.Permissions
{
    public enum OnboardingPermissions
    {
        [Display(Name = "Can Manage Location")]
        CanManageLocation,

        [Display(Name = "Can View Onboarding Request")]
        CanViewOnboardingRequest,

        [Display(Name = "Can Delete Onboarding Request")]
        CanDeleteOnboardingRequest,

        [Display(Name = "Can Generate OD Letter")]
        CanGenrateOdLetter,

        [Display(Name = "Can Assign OD Account")]
        CanAssignOdAccount,

        [Display(Name = "Can Assign KO Code")]
        CanAssignKoCode
    }
}
