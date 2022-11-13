using AutoMapper;
using Contracts.DTOs;
using Domain.Entities;

namespace Infrastructure.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductDto>();

            CreateMap<User, UserDto>().ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.Email));

            CreateMap<CartItem, CartItemDto>();

            CreateMap<Cart, CartDto>();            
        }
    }
}
