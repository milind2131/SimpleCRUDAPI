using Dapper;
using SimpleCRUDAPI.Ecommerce.Application.Interfaces;
using SimpleCRUDAPI.Ecommerce.Domain.Constants;
using SimpleCRUDAPI.Ecommerce.Infrastructure.Data;
using SimpleCRUDAPI.Model;
using System.Data;

namespace SimpleCRUDAPI.Ecommerce.Infrastructure.Repository;

public class ProductRepository : IProductRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ProductRepository(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        using var connection =
            _connectionFactory.CreateConnection();

        return await connection.QueryAsync<Product>(
            ProductStoredProcedures.GetAll,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Product?> GetByIdAsync(
        int productId)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Product>(
            ProductStoredProcedures.GetById,
            new
            {
                ProductId = productId
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Product>> GetByOwnerAsync(
        int ownerUserId)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        return await connection.QueryAsync<Product>(
            ProductStoredProcedures.GetByOwner,
            new
            {
                OwnerUserId = ownerUserId
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> AddAsync(
        Product product,
        int ownerUserId)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            ProductStoredProcedures.Insert,
            new
            {
                ProductName = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Description = product.Description,
                StockQuantity = product.StockQuantity,
                OwnerUserId = ownerUserId
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> UpdateAsync(
        Product product,
        int currentUserId,
        bool isSuperAdmin)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            ProductStoredProcedures.Update,
            new
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Description = product.Description,
                StockQuantity = product.StockQuantity,
                CurrentUserId = currentUserId,
                IsSuperAdmin = isSuperAdmin
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> UpdateProductImageAsync(
        int productId,
        string imageUrl,
        int currentUserId,
        bool isSuperAdmin)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            ProductStoredProcedures.UpdateImage,
            new
            {
                ProductId = productId,
                ImageUrl = imageUrl,
                CurrentUserId = currentUserId,
                IsSuperAdmin = isSuperAdmin
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> DeleteAsync(
        int productId)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            ProductStoredProcedures.Delete,
            new
            {
                ProductId = productId
            },
            commandType: CommandType.StoredProcedure);
    }
}