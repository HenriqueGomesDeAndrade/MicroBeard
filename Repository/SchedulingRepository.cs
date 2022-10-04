using Entities;
using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;

namespace MicroBeard.Repository
{
    public class SchedulingRepository : RepositoryBase<Scheduling>, ISchedulingRepository
    {
        public SchedulingRepository(RepositoryContext respositoryContext)
            :base(respositoryContext)
        {
            
        }
    }
}
