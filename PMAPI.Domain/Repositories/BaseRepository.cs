using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PMAPI.Data.Contexts;
using PMAPI.Data.Models;

namespace PMAPI.Domain.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly DataContext _context;
        private readonly DbSet<T> _entities;
        //private readonly IErrorHandler _errorHandler;

        public BaseRepository(DataContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _entities.ToListAsync<T>();
        }

        public async Task<T> GetById(int id)
        {
            return await _entities.SingleOrDefaultAsync(e => e.ID == id);
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> exp)
        {
            return _entities.Where(exp);
        }

        public async void Insert(T entity)
        {
            if (entity == null)
            {
                //throw new ArgumentNullException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.EntityNull), "", "Input data is null"));
            }

            await _entities.AddAsync(entity);
            _context.SaveChanges();
        }

        public async void Update(T entity)
        {
            if (entity == null) 
            {
                //throw new ArgumentNullException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.EntityNull), "", "Input data is null"));
            }

            var oldEntity = await _context.FindAsync<T>(entity.ID);
            _context.Entry(oldEntity).CurrentValues.SetValues(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                //throw new ArgumentNullException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.EntityNull), "", "Input data is null"));
            }

            _entities.Remove(entity);
            _context.SaveChanges();
        }
    }
}
