using SimpleCRUDAPI.Model;

namespace SimpleCRUDAPI.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAll();

        Task<Product?> GetProductById(int id);

        Task<Product> AddProduct(Product product);

        Task<Product?> UpdateProduct(Product product);

        Task<bool> DeleteProduct(int id);
    }
}
