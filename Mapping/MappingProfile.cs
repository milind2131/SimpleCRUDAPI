using AutoMapper;
using SimpleCRUDAPI.DTO_s;
using SimpleCRUDAPI.Model;

namespace SimpleCRUDAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Product,ProductRequestDto>();
            CreateMap<Product,ProductResponseDto>();
            CreateMap<ProductRequestDto,Product >();
            CreateMap<ProductRequestDto,Product>();
            
        }
    }
}
