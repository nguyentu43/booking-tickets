using AutoMapper;
using BookingTickets.Data.Base;
using System.Threading.Tasks;

namespace BookingTickets.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _dbContext;
        public IGenreRepository GenreRepository { get; private set; }
        public IMovieRepository MovieRepository { get; private set; }
        public IMovieGenreRepository MovieGenreRepository { get; private set; }
        public ICinemaRepository CinemaRepository { get; private set; }
        public IRoomRepository RoomRepository { get; private set; }
        public ISeatRepository SeatRepository { get; private set; }
        public IScreeningRepository ScreeningRepository { get; private set; }
        public IReservationRepository ReservationRepository { get; private set; }
        public IReservationSeatRepository ReservationSeatRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }
        public UnitOfWork(ApplicationDbContext dbContext, IMapper _mapper)
        {
            _dbContext = dbContext;
            GenreRepository = new GenreRepository(_dbContext, _mapper);
            MovieRepository = new MovieRepository(_dbContext, _mapper);
            MovieGenreRepository = new MovieGenreRepository(_dbContext, _mapper);
            CinemaRepository = new CinemaRepository(_dbContext, _mapper);
            RoomRepository = new RoomRepository(_dbContext, _mapper);
            SeatRepository = new SeatRepository(_dbContext, _mapper);
            ScreeningRepository = new ScreeningRepository(_dbContext, _mapper);
            ReservationRepository = new ReservationRepository(_dbContext, _mapper);
            ReservationSeatRepository = new ReservationSeatRepository(_dbContext, _mapper);
            UserRepository = new UserRepository(_dbContext, _mapper);
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
