using Entities;
using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;

namespace MicroBeard.Repository
{
    public class ServiceRepository : RepositoryBase<Service>, IServiceRepository
    {
        public ServiceRepository(RepositoryContext respositoryContext)
            :base(respositoryContext)
        {
            
        }
    }
}
