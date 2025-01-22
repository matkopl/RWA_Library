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

            CreateMap<BookDto, BookVM>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
                .ReverseMap();

            CreateMap<Book, BookVM>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name))
                .ReverseMap();
        }
    }
}
