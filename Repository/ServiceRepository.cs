using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;
using System.ComponentModel;
using System.Data.Entity;


namespace MicroBeard.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        private MicroBeardContext _repositoryContext { get; set; }

        public ServiceRepository(MicroBeardContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IEnumerable<Service> GetAllServices()
        {
            return _repositoryContext.Services.AsNoTracking()
                .Where(c => c.Deleted != true)
                .OrderBy(c => c.Name)
                .ToList();
        }

        public Service GetServiceByCode(int code)
        {
            Service service = _repositoryContext.Services.AsNoTracking().Where(c => c.Deleted != true && c.Code.Equals(code)).FirstOrDefault();

            _repositoryContext.Entry(service).Collection(c => c.Schedulings).Load();
            _repositoryContext.Entry(service).Collection(c => c.Collaborators).Load();

            return service;
        }

        public void CreateService(Service Service)
        {
            _repositoryContext.Services.Add(Service);
        }

        public void UpdateService(Service Service)
        {
            _repositoryContext.Services.Update(Service);
        }

        public void DeleteService(Service Service)
        {
            _repositoryContext.Services.Remove(Service);
        }
    }
}
