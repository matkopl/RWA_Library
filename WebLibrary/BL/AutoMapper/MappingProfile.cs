using AutoMapper;
using BL.DTO;
using BL.Models;
using BL.Viewmodels;

namespace BL.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>()
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name))
            .ForMember(dest => dest.GenreId, opt => opt.MapFrom(src => src.GenreId))
            .ReverseMap();

            CreateMap<RegisterDto, User>();

            CreateMap<CreateUpdateBookDto, Book>()
                .ForMember(dest => dest.GenreId, opt => opt.MapFrom(src => src.GenreId))
                .ReverseMap();

            CreateMap<BookDto, BookIndexVM>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
                .ReverseMap();

            CreateMap<Book, BookIndexVM>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name))
                .ReverseMap();

            CreateMap<Book, BookVM>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name))
                .ReverseMap();

            CreateMap<CreateBookVM, Book>()
                .ForMember(dest => dest.GenreId, opt => opt.MapFrom(src => src.GenreId))
                .ReverseMap();

            CreateMap<UpdateBookVM, Book>()
                .ForMember(dest => dest.GenreId, opt => opt.MapFrom(src => src.GenreId))
                .ReverseMap();

            CreateMap<RegisterVM, User>();

            CreateMap<CreateUpdateBookDto, CreateBookVM>()
                .ReverseMap();

            CreateMap<Reservation, ReservationVM>()
                .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.Book.Name))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Book.Genre.Name)) 
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location.Name))
                .ReverseMap();

            CreateMap<CreateReservationVM, Reservation>()
                .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.LocationId))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId))
                .ReverseMap();

            CreateMap<UserDetailsVM, User>().ReverseMap();

            CreateMap<Genre, GenreVM>().ReverseMap();

            CreateMap<Location, LocationVM>().ReverseMap();

            CreateMap<Location, LocationCrudVM>().ReverseMap();
        }
    }
}
