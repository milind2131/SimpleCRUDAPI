namespace SimpleCRUDAPI.Ecommerce.Domain.Constants
{
    public static class RoleUpgradeStoredProcedures
    {
        public const string Create =
            "Security.usp_CreateRoleUpgradeRequest";

        public const string GetLatestByUserId =
            "Security.usp_GetLatestRoleUpgradeRequestByUserId";

        public const string GetPending =
            "Security.usp_GetPendingRoleUpgradeRequests";

        public const string Approve =
            "Security.usp_ApproveRoleUpgradeRequest";

        public const string Reject =
            "Security.usp_RejectRoleUpgradeRequest";
    }
}
