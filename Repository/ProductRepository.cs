using SimpleCRUDAPI.Model;

namespace SimpleCRUDAPI.Repository
{
    public class ProductRepository : IProductRepository
    {

        private static List<Product> products =
    [
        new Product { Id = 1, Name = "Laptop", Price = 65000, Category = "Electronics" },
        new Product { Id = 2, Name = "Mobile", Price = 25000, Category = "Electronics" },
        new Product { Id = 3, Name = "Shoes", Price = 2000, Category = "Fashion" }
    ];

        public Task<List<Product>> GetAll()
        {
            //return Task.FromResult(products);
            throw new Exception("This is a test exception from Repository.");
        }

        public Task<Product?> GetProductById(int id)
        {
            var product = products.FirstOrDefault(x => x.Id == id);

            return Task.FromResult(product);
        }

        public Task<Product> AddProduct(Product product)
        {
            product.Id = products.Max(x => x.Id) + 1;

            products.Add(product);

            return Task.FromResult(product);
        }

        public Task<Product?> UpdateProduct(Product product)
        {
            var existing = products.FirstOrDefault(x => x.Id == product.Id);

            if (existing == null)
                return Task.FromResult<Product?>(null);

            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.Category = product.Category;

            return Task.FromResult<Product?>(existing);
        }

        public Task<bool> DeleteProduct(int id)
        {
            var product = products.FirstOrDefault(x => x.Id == id);

            if (product == null)
                return Task.FromResult(false);

            products.Remove(product);

            return Task.FromResult(true);
        }
    }
}
