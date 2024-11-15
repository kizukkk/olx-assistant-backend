using AutoMapper;
using olx_assistant_application.DTOs.Shared;
using olx_assistant_domain.Entities;

namespace olx_assistant_application.Mapper;
public class MapperProfile: Profile
{
    public MapperProfile()
    {
        CreateMap<Product, ProductResponse>();
    }
}
