using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;
using MicroBeard.Entities.Parameters;
using MicroBeard.Helpers;
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

        public PagedList<Service> GetAllServices(ServiceParameters serviceParameters)
        {
            return PagedList<Service>.ToPagedList(
                _repositoryContext.Services.AsNoTracking()
                .Where(c => c.Deleted != true)
                .OrderBy(c => c.Name),
                serviceParameters.PageNumber,
                serviceParameters.PageSize);
        }

        public Service GetServiceByCode(int code, bool expandRelations = false)
        {
            Service service = _repositoryContext.Services.Where(c => c.Deleted != true && c.Code.Equals(code)).FirstOrDefault();

            if (service != null && expandRelations)
            {
                _repositoryContext.Entry(service).Collection(c => c.Schedulings).Load();
                _repositoryContext.Entry(service).Collection(c => c.Collaborators).Load();
            }

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
