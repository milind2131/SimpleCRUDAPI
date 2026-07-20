using SimpleCRUDAPI.DTO_s;
using SimpleCRUDAPI.Model;

namespace SimpleCRUDAPI.Services
{
    public interface IProductService
    {
       
       

        Task<List<ProductResponseDto>> GetAll();

        Task<ProductResponseDto?> GetById(int id);

        Task<ProductResponseDto> Add(ProductRequestDto request);

        Task<ProductResponseDto?> Update(int id, ProductRequestDto request);

        Task<bool> Delete(int id);
    }
}
