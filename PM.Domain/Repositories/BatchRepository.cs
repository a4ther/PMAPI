using System;
using PM.Data.Contexts;
using PM.Data.Models;

namespace PM.Domain.Repositories
{
    public class BatchRepository : BaseRepository<Batch>, IBatchRepository
    {
        public BatchRepository(DataContext context)
            : base(context)
        {
        }
    }
}
