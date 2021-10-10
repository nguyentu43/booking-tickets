using BookingTickets.Models;
using BookingTickets.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTickets.Data.Base
{
    public interface IMovieRepository:IRepository<Movie>
    {
        Task<Movie> Add(MovieVM movieVM);
        Task<Movie> Update(int id, MovieVM movieVM);
        Task Remove(int id);
        Task<Movie> GetById(int id);
    }
}
