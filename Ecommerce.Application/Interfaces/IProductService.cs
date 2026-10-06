using SimpleCRUDAPI.DTO_s;

namespace SimpleCRUDAPI.Ecommerce.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductResponseDto>>
        GetAllProductsAsync();

    Task<ProductResponseDto?>
        GetProductByIdAsync(int productId);

    Task<IEnumerable<ProductResponseDto>>
        GetMyProductsAsync(int userId);

    Task<ProductResponseDto?>
        AddProductAsync(
            ProductRequestDto request,
            int ownerUserId);

    Task<bool>
        UpdateProductAsync(
            int productId,
            ProductRequestDto request,
            int currentUserId,
            bool isSuperAdmin);

    Task<bool>
        UpdateProductImageAsync(
            int productId,
            string imageUrl,
            int currentUserId,
            bool isSuperAdmin);

    Task<bool>
        CanModifyProductAsync(
            int productId,
            int currentUserId,
            bool isSuperAdmin);

    Task<bool>
        DeleteProductAsync(int productId);
}