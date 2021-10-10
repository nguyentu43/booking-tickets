using AutoMapper;
using BookingTickets.Data.Base;
using BookingTickets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTickets.Data
{
    public class CinemaRepository : Repository<Cinema>, ICinemaRepository
    {
        public CinemaRepository(ApplicationDbContext _dbContext, IMapper _mapper):base(_dbContext, _mapper) { }
    }
}
