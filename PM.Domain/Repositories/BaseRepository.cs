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
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly DataContext _context;
        private readonly DbSet<T> _entities;

        public BaseRepository(DataContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public Task<List<T>> GetAllAsync()
        {
            return _entities.ToListAsync<T>();
        }

        public Task<T> GetByIdAsync(int id)
        {
            return _entities.SingleOrDefaultAsync(e => e.ID == id);
        }

        public Task<List<T>> WhereAsync(Expression<Func<T, bool>> exp)
        {
            return _entities.Where(exp).ToListAsync();
        }

        public async Task<T> InsertAsync(T entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public void Update(T entity)
        {
            var oldEntity =  _context.Find<T>(entity.ID);
            _context.Entry(oldEntity).CurrentValues.SetValues(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _entities.Remove(entity);
            _context.SaveChanges();
        }
    }
}
