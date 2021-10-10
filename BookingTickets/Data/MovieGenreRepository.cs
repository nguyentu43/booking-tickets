using AutoMapper;
using BookingTickets.Data.Base;
using BookingTickets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTickets.Data
{
    public class MovieGenreRepository : Repository<MovieGenre>, IMovieGenreRepository
    {
        public MovieGenreRepository(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}
