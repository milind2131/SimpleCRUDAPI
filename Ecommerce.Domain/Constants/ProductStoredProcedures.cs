namespace SimpleCRUDAPI.Ecommerce.Domain.Constants
{
    public class ProductStoredProcedures
    {

        public const string GetAll =
        "Catalog.usp_GetAllProducts";

        public const string GetById =
            "Catalog.usp_GetProductById";

        public const string GetByOwner =
            "Catalog.usp_GetProductsByOwner";

        public const string Insert =
            "Catalog.usp_InsertProduct";

        public const string Update =
            "Catalog.usp_UpdateProduct";

        public const string Delete =
            "Catalog.usp_DeleteProduct";

        public const string UpdateImage =
            "Catalog.usp_UpdateProductImage";
    }
}
