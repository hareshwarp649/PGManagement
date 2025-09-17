using System.ComponentModel;

namespace PropertyManage.Domain.Enums
{
    public enum UserRoleEnum
    {
        [Description("Bank Mitra")]
        BCA,

        [Description("Bc Branch Operator")]
        BC_BRANCH_OPERATOR,

        [Description("Junior Territory Manager")]
        JR_TERRITORY_MANAGER,

        [Description("Territory Manager")]
        TERRITORY_MANAGER,

        [Description("Senior Territory Manager")]
        SR_TERRITORY_MANAGER,

        [Description("Junior Area Manager")]
        JR_AREA_MANAGER,

        [Description("Area Manager")]
        AREA_MANAGER,

        [Description("Senior Area Manager")]
        SR_AREA_MANAGER,

        [Description("Junior Regional Manager")]
        JR_REGIONAL_MANAGER,

        [Description("Regional Manager")]
        REGIONAL_MANAGER,

        [Description("Senior Regional Manager")]
        SR_REGIONAL_MANAGER,

        [Description("State Head")]
        STATE_HEAD,

        [Description("Bank Single Point of Contact (IN HOUSE)")]
        SPOC_IN_HOUSE,

        [Description("Admin")]
        ADMIN,

        [Description("Super Admin")]
        SUPER_ADMIN,

        [Description("Cluster Head")]
        CLUSTER_HEAD,

        [Description("Executive - MIS & Reporting")]
        EXECUTIVE_MIS_REPORTING,

        [Description("Junior Executive")]
        JR_EXECUTIVE,

        [Description("Junior Executive - Operation Support")]
        JR_EXECUTIVE_OPERATION_SUPPORT,

        [Description("Manager - Business Development")]
        MANAGER_BUSINESS_DEVELOPMENT,

        [Description("Team Leader")]
        TEAM_LEADER,

        [Description("Telecaller")]
        TELECALLER
    }
}
