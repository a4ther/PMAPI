using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PM.Domain.Models;

namespace PM.Domain.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetAsync();

        Task<CategoryResponse> GetByIdAsync(int id);

        Task<List<CategoryResponse>> GetSubcategoriesAsync(int id);

        Task<CategoryResponse> AddAsync(CategoryResponse entry);

        Task<CategoryResponse> UpdateAsync(CategoryResponse entry);

        Task<CategoryResponse> RemoveAsync(int id);
    }
}
