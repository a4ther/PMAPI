using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PM.Data.Models;

namespace PM.Domain.Services
{
    public interface IBaseService<T> where T : BaseEntity
    {
        Task<List<T>> GetAsync();

        Task<T> GetByIdAsync(int id);

        Task<List<T>> WhereAsync(Expression<Func<T, bool>> exp);

        Task<T> AddAsync(T entry);

        Task<T> UpdateAsync(T entry);

        Task<T> RemoveAsync(int id);
    }
}
