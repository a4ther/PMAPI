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

        public async Task<CategoryResponse> AddAsync(CategoryResponse entry)
        {
            var category = _mapper.Map<CategoryResponse, Category>(entry);
            category = await _service.AddAsync(category);
            if (category.ParentID.HasValue)
            {
                category.Parent = await _service.GetByIdAsync(category.ParentID.Value);
            }
            return _mapper.Map<Category, CategoryResponse>(category);
        }

        public async Task<List<CategoryResponse>> GetAsync()
        {
            var categories = await _service.GetAsync();
            return _mapper.Map<List<Category>, List<CategoryResponse>>(categories);
        }

        public async Task<CategoryResponse> GetByIdAsync(int id)
        {
            var category = await _service.GetByIdAsync(id);
            if (category.ParentID.HasValue)
            {
                category.Parent = await _service.GetByIdAsync(category.ParentID.Value);
            }
            return _mapper.Map<Category, CategoryResponse>(category);
        }

        public async Task<List<CategoryResponse>> GetSubcategoriesAsync(int id)
        {
            var subcategories = await _service.WhereAsync(c => c.ParentID == id);
            return _mapper.Map<List<Category>, List<CategoryResponse>>(subcategories);
        }

        public async Task<CategoryResponse> RemoveAsync(int id)
        {
            var category = await _service.RemoveAsync(id);
            return _mapper.Map<Category, CategoryResponse>(category);
        }

        public async Task<CategoryResponse> UpdateAsync(CategoryResponse entry)
        {
            var category = _mapper.Map<CategoryResponse, Category>(entry);
            if (!category.ParentID.HasValue)
            {
                var entity = await _service.GetByIdAsync(category.ID);
                category.ParentID = entity.ParentID;
            }

            category = await _service.UpdateAsync(category);
            return _mapper.Map<Category, CategoryResponse>(category);
        }
    }
}
