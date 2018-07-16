using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PM.Data.Contexts;
using PM.Data.Models;

namespace PM.Domain.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly DataContext _context;
        protected readonly DbSet<T> _entities;

        protected BaseRepository(DataContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public virtual Task<List<T>> GetAllAsync()
        {
            return _entities.ToListAsync<T>();
        }

        public virtual Task<T> GetByIdAsync(int id)
        {
            return _entities.SingleOrDefaultAsync(e => e.ID == id);
        }

        public virtual Task<List<T>> WhereAsync(Expression<Func<T, bool>> exp)
        {
            return _entities.Where(exp).ToListAsync();
        }

        public virtual async Task<T> InsertAsync(T entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual void Update(T entity)
        {
            var oldEntity =  _context.Find<T>(entity.ID);
            _context.Entry(oldEntity).CurrentValues.SetValues(entity);
            _context.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            _entities.Remove(entity);
            _context.SaveChanges();
        }
    }
}
