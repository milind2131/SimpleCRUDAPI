using SimpleCRUDAPI.Model;

namespace SimpleCRUDAPI.Ecommerce.Application.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();

    Task<Product?> GetByIdAsync(int productId);

    Task<IEnumerable<Product>> GetByOwnerAsync(
        int ownerUserId);

    Task<int> AddAsync(
        Product product,
        int ownerUserId);

    Task<int> UpdateAsync(
        Product product,
        int currentUserId,
        bool isSuperAdmin);

    Task<int> UpdateProductImageAsync(
        int productId,
        string imageUrl,
        int currentUserId,
        bool isSuperAdmin);

    Task<int> DeleteAsync(int productId);
}