using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PM.Data.Models;
using PM.Domain.Models;

namespace PM.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IBaseService<Category> _service;
        private readonly IMapper _mapper;

        public CategoryService(IBaseService<Category> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<CategoryDTO> AddAsync(CategoryDTO entry)
        {
            var category = _mapper.Map<CategoryDTO, Category>(entry);
            category = await _service.AddAsync(category);
            if (category.ParentID.HasValue)
            {
                category.Parent = await _service.GetByIdAsync(category.ParentID.Value);
            }
            return _mapper.Map<Category, CategoryDTO>(category);
        }

        public async Task<List<CategoryDTO>> GetAsync()
        {
            var categories = await _service.GetAsync();
            return _mapper.Map<List<Category>, List<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO> GetByIdAsync(int id)
        {
            var category = await _service.GetByIdAsync(id);
            if (category.ParentID.HasValue)
            {
                category.Parent = await _service.GetByIdAsync(category.ParentID.Value);
            }
            return _mapper.Map<Category, CategoryDTO>(category);
        }

        public async Task<List<CategoryDTO>> GetSubcategoriesAsync(int id)
        {
            var subcategories = await _service.WhereAsync(c => c.ParentID == id);
            return _mapper.Map<List<Category>, List<CategoryDTO>>(subcategories);
        }

        public async Task<CategoryDTO> RemoveAsync(int id)
        {
            var category = await _service.RemoveAsync(id);
            return _mapper.Map<Category, CategoryDTO>(category);
        }

        public async Task<CategoryDTO> UpdateAsync(CategoryDTO entry)
        {
            var category = _mapper.Map<CategoryDTO, Category>(entry);
            if (!category.ParentID.HasValue)
            {
                var entity = await _service.GetByIdAsync(category.ID);
                category.ParentID = entity.ParentID;
            }

            category = await _service.UpdateAsync(category);
            return _mapper.Map<Category, CategoryDTO>(category);
        }
    }
}
