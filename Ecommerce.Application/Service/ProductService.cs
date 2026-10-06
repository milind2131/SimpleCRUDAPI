using SimpleCRUDAPI.DTO_s;
using SimpleCRUDAPI.Ecommerce.Application.Interfaces;
using SimpleCRUDAPI.Model;

namespace SimpleCRUDAPI.Ecommerce.Application.Service;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(
        IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductResponseDto>>
        GetAllProductsAsync()
    {
        var products =
            await _productRepository.GetAllAsync();

        return products.Select(MapToResponse);
    }

    public async Task<ProductResponseDto?>
        GetProductByIdAsync(
            int productId)
    {
        var product =
            await _productRepository.GetByIdAsync(
                productId);

        if (product == null)
        {
            return null;
        }

        return MapToResponse(product);
    }

    public async Task<IEnumerable<ProductResponseDto>>
        GetMyProductsAsync(
            int userId)
    {
        var products =
            await _productRepository.GetByOwnerAsync(
                userId);

        return products.Select(MapToResponse);
    }

    public async Task<ProductResponseDto?>
        AddProductAsync(
            ProductRequestDto request,
            int ownerUserId)
    {
        var product = new Product
        {
            Name = request.Name.Trim(),
            Price = request.Price,
            CategoryId = request.CategoryId,
            Description = request.Description?.Trim(),
            StockQuantity = request.StockQuantity
        };

        var productId =
            await _productRepository.AddAsync(
                product,
                ownerUserId);

        if (productId <= 0)
        {
            return null;
        }

        var createdProduct =
            await _productRepository.GetByIdAsync(
                productId);

        if (createdProduct == null)
        {
            return null;
        }

        return MapToResponse(createdProduct);
    }

    public async Task<bool>
        UpdateProductAsync(
            int productId,
            ProductRequestDto request,
            int currentUserId,
            bool isSuperAdmin)
    {
        var product = new Product
        {
            Id = productId,
            Name = request.Name.Trim(),
            Price = request.Price,
            CategoryId = request.CategoryId,
            Description = request.Description?.Trim(),
            StockQuantity = request.StockQuantity
        };

        var affectedRows =
            await _productRepository.UpdateAsync(
                product,
                currentUserId,
                isSuperAdmin);

        return affectedRows > 0;
    }

    public async Task<bool>
        UpdateProductImageAsync(
            int productId,
            string imageUrl,
            int currentUserId,
            bool isSuperAdmin)
    {
        var affectedRows =
            await _productRepository.UpdateProductImageAsync(
                productId,
                imageUrl,
                currentUserId,
                isSuperAdmin);

        return affectedRows > 0;
    }

    public async Task<bool>
        CanModifyProductAsync(
            int productId,
            int currentUserId,
            bool isSuperAdmin)
    {
        if (isSuperAdmin)
        {
            return true;
        }

        var product =
            await _productRepository.GetByIdAsync(
                productId);

        if (product == null)
        {
            return false;
        }

        return product.OwnerUserId ==
               currentUserId;
    }

    public async Task<bool>
        DeleteProductAsync(
            int productId)
    {
        var affectedRows =
            await _productRepository.DeleteAsync(
                productId);

        return affectedRows > 0;
    }

    private static ProductResponseDto MapToResponse(
        Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            CategoryId = product.CategoryId,
            Category = product.Category,
            Description = product.Description,
            StockQuantity = product.StockQuantity,
            ImageUrl = product.ImageUrl
        };
    }
}