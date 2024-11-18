using AutoMapper;
using Backend_Teamwork.src.Entities;
using static Backend_Teamwork.src.DTO.ArtworkDTO;
using static Backend_Teamwork.src.DTO.BookingDTO;
using static Backend_Teamwork.src.DTO.CategoryDTO;
using static Backend_Teamwork.src.DTO.OrderDetailDTO;
using static Backend_Teamwork.src.DTO.OrderDTO;
using static Backend_Teamwork.src.DTO.PaymentDTO;
using static Backend_Teamwork.src.DTO.UserDTO;
using static Backend_Teamwork.src.DTO.WorkshopDTO;

namespace Backend_Teamwork.src.Utils
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>()
                .ForMember(
                    dest => dest.Description,
                    opt => opt.Condition(src => src.Description != null)
                )
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcProperty) => srcProperty != null)
                );

            CreateMap<Artwork, ArtworkReadDto>();
            CreateMap<ArtworkCreateDto, Artwork>();
            CreateMap<ArtworkUpdateDTO, Artwork>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcProperty) => srcProperty != null)
                );

            CreateMap<Category, CategoryReadDto>();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcProperty) => srcProperty != null)
                );

            CreateMap<Order, OrderReadDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
            CreateMap<OrderCreateDto, Order>();
            CreateMap<OrderUpdateDto, Order>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcProperty) => srcProperty != null)
                );

            CreateMap<Payment, PaymentReadDTO>();
            CreateMap<PaymentCreateDTO, Payment>();
            CreateMap<PaymentReadDTO, Payment>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcProperty) => srcProperty != null)
                );

            CreateMap<Workshop, WorkshopReadDTO>();
            CreateMap<WorkshopCreateDTO, Workshop>();
            CreateMap<WorkshopUpdateDTO, Workshop>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcProperty) => srcProperty != null)
                );

            CreateMap<OrderDetails, OrderDetailReadDto>();
            CreateMap<OrderDetailCreateDto, OrderDetails>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcProperty) => srcProperty != null)
                );

            CreateMap<Booking, BookingReadDto>();
            CreateMap<BookingCreateDto, Booking>();
        }
    }
}
