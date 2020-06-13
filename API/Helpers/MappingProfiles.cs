using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(dest => dest.ProductBrand, o => o.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.ProductType, o => o.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.PictureUrl, o => o.MapFrom<ProductUrlResolver>());

            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(dest => dest.DeliveryMethod, o => o.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.ShippingPrice, o => o.MapFrom(src => src.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductId, o => o.MapFrom(src => src.ItemOrdered.ProductItemId))
                .ForMember(dest => dest.ProductName, o => o.MapFrom(src => src.ItemOrdered.ProductName))
                .ForMember(dest => dest.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());
        }
    }
}