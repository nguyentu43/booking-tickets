using BookingTickets.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTickets.Data.Base
{
    public interface IUnitOfWork
    {
        IGenreRepository GenreRepository { get; }
        IMovieRepository MovieRepository { get; }
        IMovieGenreRepository MovieGenreRepository { get; }
        ICinemaRepository CinemaRepository { get; }
        IRoomRepository RoomRepository { get; }
        ISeatRepository SeatRepository { get; }
        IScreeningRepository ScreeningRepository { get; }
        IReservationRepository ReservationRepository { get; }
        IReservationSeatRepository ReservationSeatRepository { get; }
        Task<int> SaveChangeAsync();
    }
}
