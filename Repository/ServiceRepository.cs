using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;
using System.Data.Entity;


namespace MicroBeard.Repository
{
    public class ServiceRepository : RepositoryBase<Service>, IServiceRepository
    {
        public ServiceRepository(MicroBeardContext respositoryContext)
            :base(respositoryContext)
        {
            
        }

        public IEnumerable<Service> GetAllServices()
        {
            return FindAll()
                .Where(c => c.Deleted != true)
                .OrderBy(c => c.Name)
                .ToList();
        }

        public Service GetServiceByCode(int code)
        {
            return FindByCondition(c => c.Deleted != true && c.Code.Equals(code)).FirstOrDefault();
        }

        public Service GetServiceWithDetails(int code)
        {
            return FindByCondition(c => c.Deleted != true && c.Code.Equals(code)).Include(s => s.Schedulings).FirstOrDefault();
        }

        public void CreateService(Service Service)
        {
            Create(Service);
        }

        public void UpdateService(Service Service)
        {
            Update(Service);
        }

        public void DeleteService(Service Service)
        {
            Delete(Service);
        }
    }
}
