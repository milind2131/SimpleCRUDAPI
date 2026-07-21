using SimpleCRUDAPI.Model;

namespace SimpleCRUDAPI.Ecommerce.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task<Product?> GetProductByIdAsync(int productId);

        Task<int> InsertProductAsync(Product product);

        Task<int> UpdateProductAsync(Product product);

        Task<int> DeleteProductAsync(int productId);
    }
}
