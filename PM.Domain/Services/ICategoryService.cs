using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PM.Domain.Models;

namespace PM.Domain.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAsync();

        Task<CategoryDTO> GetByIdAsync(int id);

        Task<List<CategoryDTO>> GetSubcategoriesAsync(int id);

        Task<CategoryDTO> AddAsync(CategoryDTO entry);

        Task<CategoryDTO> UpdateAsync(CategoryDTO entry);

        Task<CategoryDTO> RemoveAsync(int id);
    }
}
