using AutoMapper;
using Martiello.Application.UseCases.Product.CreateProduct;
using Martiello.Application.UseCases.Product.UpdateProduct;
using Martiello.Domain.DTO;
using Martiello.Domain.Entity;
using Martiello.Domain.Extension;

namespace Martiello.Application.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductInput, Product>().ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.GetDescription()));
            CreateMap<Product, ProductDTO>();
            CreateMap<UpdateProductInput, Product>().ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<Product, ProductOrderDTO>();

        }
    }
}
