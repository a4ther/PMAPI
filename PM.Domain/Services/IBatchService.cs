using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PM.Data.Models;
using PM.Domain.Models;

namespace PM.Domain.Services
{
    public interface IBatchService : IBaseService<BatchDTO, Batch>
    {
        Task<List<BatchDTO>> GetByDateAsync(DateTime fromDate, DateTime toDate);
    }
}
