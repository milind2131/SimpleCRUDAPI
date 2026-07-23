namespace SimpleCRUDAPI.Ecommerce.Infrastructure.Constants
{
    public static class StoredProcedures
    {
        public const string GetAllProducts = "Catalog.usp_GetAllProducts";

        public const string GetProductById = "Catalog.usp_GetProductById";

        public const string InsertProduct = "Catalog.usp_InsertProduct";

        public const string UpdateProduct = "Catalog.usp_UpdateProduct";

        public const string DeleteProduct = "Catalog.usp_DeleteProduct";

        public const string CheckUserExistsByEmail = "Security.usp_CheckUserExistsByEmail";

        public const string RegisterUser = "Security.usp_RegisterUser";

        public const string GetUserByEmail = "Security.usp_GetUserByEmail";
    }
}
