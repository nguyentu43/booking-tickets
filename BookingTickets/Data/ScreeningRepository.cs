using AutoMapper;
using BookingTickets.Data.Base;
using BookingTickets.Models;
using BookingTickets.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTickets.Data
{
    public class ScreeningRepository:Repository<Screening>, IScreeningRepository
    {
        public ScreeningRepository(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}
