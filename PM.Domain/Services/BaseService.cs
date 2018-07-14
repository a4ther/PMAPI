using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using PM.Data.Models;
using PM.Domain.Models;
using PM.Domain.Repositories;

namespace PM.Domain.Services
{
    public abstract class BaseService<TDto, TEntity> : IBaseService<TDto, TEntity>
        where TDto : BaseDTO
        where TEntity : BaseEntity
    {
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        protected BaseService(IBaseRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<TDto> AddAsync(TDto entry)
        {
            var entity = _mapper.Map<TDto, TEntity>(entry);

            entity.DateAdded = DateTime.UtcNow;
            entity.DateModified = entity.DateAdded;
            entity = await _repository.InsertAsync(entity);

            return _mapper.Map<TEntity, TDto>(entity);
        }

        public virtual async Task<List<TDto>> GetAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<TEntity>, List<TDto>>(entities);
        }

        public virtual async Task<TDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<TEntity, TDto>(entity);
        }

        public virtual async Task<TDto> RemoveAsync(int id)
        {
            var target =  await _repository.GetByIdAsync(id);

            if (target != null)
            {
                _repository.Delete(target);
            }

            return _mapper.Map<TEntity, TDto>(target);
        }

        public virtual async Task<TDto> UpdateAsync(TDto entry)
        {
            var target = await _repository.GetByIdAsync(entry.ID);

            if (target != null)
            {
                var entity = _mapper.Map<TDto, TEntity>(entry);

                entity.DateModified = DateTime.UtcNow;
                entity.DateAdded = target.DateAdded;
                _repository.Update(entity);
            }
            return _mapper.Map<TEntity, TDto>(target);
        }

        public virtual async Task<List<TDto>> WhereAsync(Expression<Func<TEntity, bool>> exp)
        {
            var entities = await _repository.WhereAsync(exp);
            return _mapper.Map<List<TEntity>, List<TDto>>(entities);
        }
    }
}
