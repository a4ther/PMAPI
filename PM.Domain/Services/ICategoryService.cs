using System.Collections.Generic;
using System.Threading.Tasks;
using PM.Data.Models;
using PM.Domain.Models;

namespace PM.Domain.Services
{
    public interface ICategoryService : IBaseService<CategoryDTO, Category>
    {
        Task<CategoryDTO> GetByNameAsync(string name);

        Task<List<CategoryDTO>> GetSubcategoriesAsync(int id);
    }
}
