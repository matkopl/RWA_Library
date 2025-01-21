using AutoMapper;
using BL.DTO;
using BL.Models;

namespace WebAPI.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>()
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name))
            .ForMember(dest => dest.GenreId, opt => opt.MapFrom(src => src.GenreId))
            .ReverseMap()
            .ForMember(dest => dest.Genre, opt => opt.Ignore());

            CreateMap<User, UserDto>().ReverseMap();

        }
    }
}
