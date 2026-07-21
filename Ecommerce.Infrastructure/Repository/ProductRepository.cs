using Dapper;
using SimpleCRUDAPI.Ecommerce.Application.Interfaces;
using SimpleCRUDAPI.Ecommerce.Infrastructure.Constants;
using SimpleCRUDAPI.Ecommerce.Infrastructure.Data;
using SimpleCRUDAPI.Model;
using System.Data;

namespace Ecommerce.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ProductRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<Product>(
            StoredProcedures.GetAllProducts,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Product?> GetProductByIdAsync(int productId)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Product>(
            StoredProcedures.GetProductById,
            new
            {
                ProductId = productId
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> InsertProductAsync(Product product)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            StoredProcedures.InsertProduct,
            new
            {
                product.Name,
                product.Price,
                product.Id
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> UpdateProductAsync(Product product)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteAsync(
            StoredProcedures.UpdateProduct,
            new
            {
                product.Id,
                product.Name,
                product.Price
                //product.CategoryId
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> DeleteProductAsync(int productId)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteAsync(
            StoredProcedures.DeleteProduct,
            new
            {
                ProductId = productId
            },
            commandType: CommandType.StoredProcedure);
    }
}