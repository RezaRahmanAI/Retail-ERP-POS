using AutoMapper;
using RetailERP.Application.DTOs;
using RetailERP.Domain.Entities;

namespace RetailERP.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}
