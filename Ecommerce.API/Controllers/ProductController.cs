using Microsoft.AspNetCore.Mvc;
using SimpleCRUDAPI.DTO_s;
using SimpleCRUDAPI.Ecommerce.Application.Interfaces;
using SimpleCRUDAPI.Model;

namespace SimpleCRUDAPI.Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
             var products = await _productService.GetAll();
             return Ok(products);

            throw new Exception("This is a test exception from Controller.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetById(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductRequestDto request)
        {
            var result = await _productService.Add(request);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductRequestDto request)
        {
            var result = await _productService.Update(id, request);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.Delete(id);

            if (!Convert.ToBoolean(result))
                return NotFound();

            return Ok("Product Deleted Successfully");
        }
    }
}
