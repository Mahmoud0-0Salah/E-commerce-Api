using AutoMapper;
using Entities.Models;
using Shared.DTO;
using WebApplication1.Models;

namespace BookApp
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Cateogry, CateogryDto>();
            /*    .ForCtorParam("FullAddress",
                opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));*/

            CreateMap<Product, ProductDto>();

            CreateMap<User, UserDto>();

            CreateMap<Order, OrderDto>();

            CreateMap<CateogryForCreationDto, Cateogry>();

            CreateMap<ProductForCreationDto, Product>();

            CreateMap<OrderForCreationDto, Order>();

            CreateMap<OrderDetailsForCreationDto, OrderDetails>();

            CreateMap<ProductForUpdateDto, Product>().ReverseMap();

            CreateMap<CateogryForUpdateDto, Cateogry>();

            CreateMap<UserForRegistrationDto, User>();

            CreateMap<UserDtoForUpdate, User>();
        }
    }
}
