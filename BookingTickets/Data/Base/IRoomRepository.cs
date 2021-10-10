﻿using BookingTickets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTickets.Data.Base
{
    public interface IRoomRepository:IRepository<Room>
    {
        IQueryable<Room> GetAllWithCinema();
    }
}
