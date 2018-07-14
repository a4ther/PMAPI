using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PM.Data.Models;
using PM.Domain.Models;

namespace PM.Domain.Services
{
    public interface IBaseService<TDto, TEntity> 
        where TDto : BaseDTO
        where TEntity : BaseEntity
    {
        Task<List<TDto>> GetAsync();

        Task<TDto> GetByIdAsync(int id);

        Task<List<TDto>> WhereAsync(Expression<Func<TEntity, bool>> exp);

        Task<TDto> AddAsync(TDto entry);

        Task<TDto> UpdateAsync(TDto entry);

        Task<TDto> RemoveAsync(int id);
    }
}
