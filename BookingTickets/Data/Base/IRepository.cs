using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BookingTickets.Data.Base
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IMapper Mapper { get; }
        DbSet<TEntity> DbSet { get; }
        void Add(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entity);
        IQueryable<TEntity> GetAll();
    }
}
