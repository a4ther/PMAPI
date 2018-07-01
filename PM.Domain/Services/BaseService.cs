using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PM.Data.Models;
using PM.Domain.Repositories;

namespace PM.Domain.Services
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        private readonly IBaseRepository<T> _repository;

        public BaseService(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<T> AddAsync(T entry)
        {
            entry.DateAdded = DateTime.UtcNow;
            entry.DateModified = DateTime.UtcNow;
            return await _repository.InsertAsync(entry);
        }

        public Task<List<T>> GetAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<T> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<T> RemoveAsync(int id)
        {
            var target =  _repository.GetByIdAsync(id).Result;
            if (target != null)
            {
                _repository.Delete(target);
            }
            return Task.FromResult<T>(target);
        }

        public Task<T> UpdateAsync(T entry)
        {
            var target = _repository.GetByIdAsync(entry.ID).Result;
            if (target != null)
            {
                entry.DateModified = DateTime.UtcNow;
                entry.DateAdded = target.DateAdded;
                _repository.Update(entry);
            }
            return Task.FromResult<T>(target);
        }

        public Task<List<T>> WhereAsync(Expression<Func<T, bool>> exp)
        {
            return _repository.WhereAsync(exp);
        }
    }
}
