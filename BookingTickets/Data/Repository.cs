using AutoMapper;
using BookingTickets.Data.Base;
using BookingTickets.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookingTickets.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected ApplicationDbContext _dbContext;
        public Repository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            DbSet = dbContext.Set<TEntity>();
            Mapper = mapper;
        }
        public DbSet<TEntity> DbSet { get; private set; }
        public IMapper Mapper { get; private set; }
        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbSet.AsQueryable();
        }

        public void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }
    }
}
