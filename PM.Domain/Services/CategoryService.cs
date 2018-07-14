using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PM.Data.Models;
using PM.Domain.Models;
using PM.Domain.Repositories;

namespace PM.Domain.Services
{
    public class CategoryService : BaseService<CategoryDTO, Category>, ICategoryService
    {
        public CategoryService(ICategoryRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
        }

        public override async Task<CategoryDTO> AddAsync(CategoryDTO entry)
        {
            var category = await base.AddAsync(entry);
            return await base.GetByIdAsync(category.ID);
        }

        public async Task<CategoryDTO> GetByNameAsync(string name)
        {
            var result = await base.WhereAsync(c => c.Name.Equals(name));
            return result.FirstOrDefault();
        }

        public Task<List<CategoryDTO>> GetSubcategoriesAsync(int id)
        {
            return base.WhereAsync(c => c.ParentID == id);
        }

        public override async Task<CategoryDTO> UpdateAsync(CategoryDTO entry)
        {
            var target = await _repository.GetByIdAsync(entry.ID);

            if (target != null)
            {
                var category = _mapper.Map<CategoryDTO, Category>(entry);

                if (!category.ParentID.HasValue)
                {
                    category.ParentID = target.ParentID;
                }

                category.DateModified = DateTime.UtcNow;
                category.DateAdded = target.DateAdded;
                _repository.Update(category);

                return _mapper.Map<Category, CategoryDTO>(category);
            }

            return null;
        }
    }
}
