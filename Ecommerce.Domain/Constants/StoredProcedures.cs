namespace SimpleCRUDAPI.Ecommerce.Domain.Constants
{
    public static class StoredProcedures
    {
        public const string GetAllProducts = "Catalog.usp_GetAllProducts";

        public const string GetProductById = "Catalog.usp_GetProductById";

        public const string InsertProduct = "Catalog.usp_InsertProduct";

        public const string UpdateProduct = "Catalog.usp_UpdateProduct";

        public const string DeleteProduct = "Catalog.usp_DeleteProduct";

        public const string CheckUserExistsByEmail = "Security.usp_CheckUserExistsByEmail";

        public const string InsertPendingUser = "Security.usp_InsertPendingUser";

        public const string GetPendingUserByEmail = "Security.usp_GetPendingUserByEmail";

        public const string DeletePendingUser = "Security.usp_DeletePendingUser";

        public const string UpdateRegistrationOtp = "Security.usp_UpdateRegistrationOtp";

        public const string RegisterUser = "Security.usp_RegisterUser";

        public const string GetUserByEmail = "Security.usp_GetUserByEmail";

        public const string GetUserById = "Security.usp_GetUserById";
        // Logging
        public const string InsertExceptionLog = "Logging.usp_InsertExceptionLog";

        public const string ChangePassword = "Security.usp_ChangePassword";

        // Forgot Password

        public const string InsertPasswordResetRequest = "Security.usp_InsertPasswordResetRequest";

        public const string GetPasswordResetRequest = "Security.usp_GetPasswordResetRequest";

        public const string VerifyPasswordResetRequest = "Security.usp_VerifyPasswordResetRequest";

        public const string DeletePasswordResetRequest = "Security.usp_DeletePasswordResetRequest";

        public const string UpdatePassword = "Security.usp_UpdatePassword";

        public const string UpdatePasswordResetOtp = "Security.usp_UpdatePasswordResetOtp";



    }
}
