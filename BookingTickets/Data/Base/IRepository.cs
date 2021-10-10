using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookingTickets.Data.Base
{
    public interface IRepository<TEntity> where TEntity:class
    {
        IMapper Mapper { get; }
        DbSet<TEntity> DbSet { get; }
        void Add(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entity);
        IQueryable<TEntity> GetAll();
    }
}
