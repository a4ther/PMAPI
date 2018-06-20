using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PMAPI.Data.Models;
using PMAPI.Domain.Repositories;

namespace PMAPI.Domain.Services
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        private readonly IBaseRepository<T> _repository;

        public BaseService(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        public void AddOrUpdate(T entry)
        {
            var target = _repository.GetById(entry.ID).Result;

            if (target != null)
            {
                entry.DateAdded = target.DateAdded;
                entry.DateModified = DateTime.UtcNow;
                _repository.Update(entry);
                return;
            }

            entry.DateAdded = DateTime.UtcNow;
            _repository.Insert(entry);
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _repository.GetById(id);
        }

        public void Remove(int id)
        {
            var target = _repository.GetById(id).Result;
            _repository.Delete(target);
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> exp)
        {
            return _repository.Where(exp);
        }
    }
}
