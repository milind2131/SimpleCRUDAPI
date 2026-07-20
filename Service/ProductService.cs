using AutoMapper;
using SimpleCRUDAPI.DTO_s;
using SimpleCRUDAPI.Model;
using SimpleCRUDAPI.Repository;

namespace SimpleCRUDAPI.Services
{
    public class ProductService : IProductService
    {

        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        //public Task<List<Product>> GetAll()
        //{
        //    return _productRepository.GetAll();
        //}

        // 1. Why we are using async over here without dto  working fine
        // 2. So much repeated code for object conversion any solution for that?


        /* Without using Automapper code */

        //public async Task<List<ProductResponseDto>> GetAll()  
        //{
        //    var products = await _productRepository.GetAll();

        //    return products.Select(p => new ProductResponseDto
        //    {
        //        Id = p.Id,
        //        Name = p.Name,
        //        Price = p.Price,
        //        Category = p.Category
        //    }).ToList();
        //}

        /* With using Automapper code */
        public async Task<List<ProductResponseDto>> GetAll()
        {
            var products = await _productRepository.GetAll();

            return _mapper.Map<List<ProductResponseDto>>(products);
            //throw new Exception("This is a test exception from Service.");
        }

        public async Task<ProductResponseDto?> GetById(int id)
        {
            var product = await _productRepository.GetProductById(id);

            if (product == null)
                return null;

            return _mapper.Map<ProductResponseDto>(product);
        }

        public async Task<ProductResponseDto> Add(ProductRequestDto request)
        {
            var product = _mapper.Map<Product>(request);

            var result = await _productRepository.AddProduct(product);

            return _mapper.Map<ProductResponseDto>(result);
        }

        public async Task<ProductResponseDto?> Update(int id, ProductRequestDto request)
        {
            var product = _mapper.Map<Product>(request);

            product.Id = id;

            var updated = await _productRepository.UpdateProduct(product);

            if (updated == null)
                return null;

            return _mapper.Map<ProductResponseDto>(updated);
        }

        public Task<bool> Delete(int id)
        {
            return _productRepository.DeleteProduct(id);
        }

    }
}
