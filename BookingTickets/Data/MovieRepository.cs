using AutoMapper;
using BookingTickets.Data.Base;
using BookingTickets.Models;
using BookingTickets.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTickets.Data
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public Task<Movie> Add(MovieVM movieVM)
        {
            var movie = Mapper.Map<MovieVM, Movie>(movieVM);
            Add(movie);
            return Task.FromResult(movie);
        }

        public async Task Remove(int id)
        {
            var deleteMovie = await DbSet.SingleOrDefaultAsync(g => g.Id == id);
            if (deleteMovie == null) throw new Exception("Movie not found");
            Remove(deleteMovie);
        }

        public async Task<Movie> Update(int id, MovieVM movieVM)
        {
            var movie = await DbSet.AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);
            if (movie == null) throw new Exception("Movie not found");
            movie = Mapper.Map<MovieVM, Movie>(movieVM);
            movie.Id = id;
            Update(movie);
            return movie;
        }

        public async Task<Movie> GetById(int id)
        {
            return await DbSet.Include(m => m.Genres).SingleOrDefaultAsync(m => m.Id == id);
        }
    }
}
