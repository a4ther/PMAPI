using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PM.Domain.Models;

namespace PM.Domain.Services
{
    public interface IBatchService
    {
        Task<BatchDTO> GetByIdAsync(int id);

        Task<List<BatchDTO>> GetByDateAsync(DateTime fromDate, DateTime toDate);

        Task<BatchDTO> AddAsync(BatchDTO entry);

        Task<BatchDTO> UpdateAsync(BatchDTO entry);

        Task<BatchDTO> RemoveAsync(int id);
    }
}
