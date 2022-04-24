using AutoMapper;
using BookingTickets.Models;
using BookingTickets.Models.ViewModels;
using System.Linq;

namespace BookingTickets.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Genre, GenreVM>();
            CreateMap<GenreVM, Genre>();
            CreateMap<Movie, MovieVM>()
                .ForMember(d => d.Genres, opt => opt.MapFrom(s => s.Genres.Select(g => g.Id).ToList()));
            CreateMap<MovieVM, Movie>()
                .ForMember(d => d.Genres, opt => opt.Ignore());
            CreateMap<Cinema, CinemaVM>();
            CreateMap<CinemaVM, Cinema>();
            CreateMap<Room, RoomVM>();
            CreateMap<RoomVM, Room>();
            CreateMap<Seat, SeatVM>();
            CreateMap<SeatVM, Seat>();
            CreateMap<Screening, ScreeningVM>();
            CreateMap<ScreeningVM, Screening>();
            CreateMap<ApplicationUser, AddUserVM>();
            CreateMap<AddUserVM, ApplicationUser>();
            CreateMap<ApplicationUser, UpdateUserVM>();
            CreateMap<UpdateUserVM, ApplicationUser>();
            CreateMap<RateFormVM, Rate>();
        }
    }
}
