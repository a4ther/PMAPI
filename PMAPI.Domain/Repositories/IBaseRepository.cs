using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PMAPI.Data.Models;

namespace PMAPI.Domain.Repositories
{
    public interface IBaseRepository<T>  where T : BaseEntity
    {
        Task<List<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task<List<T>> WhereAsync(Expression<Func<T, bool>> exp);

        Task<T> InsertAsync(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
