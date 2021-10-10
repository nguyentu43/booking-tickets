using AutoMapper;
using BookingTickets.Data.Base;
using BookingTickets.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTickets.Data
{
    public class RoomRepository:Repository<Room>, IRoomRepository
    {
        public RoomRepository(ApplicationDbContext _dbContext, IMapper _mapper):base(_dbContext, _mapper) { }

        public IQueryable<Room> GetAllWithCinema()
        {
            return DbSet.Include(r => r.Cinema).AsQueryable();
        }
    }
}
