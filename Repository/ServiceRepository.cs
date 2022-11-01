using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;
using MicroBeard.Entities.Parameters;
using MicroBeard.Helpers;
using MicroBeard.Helpers.Sort;
using System.ComponentModel;
using System.Data.Entity;


namespace MicroBeard.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        private MicroBeardContext _repositoryContext { get; set; }
        private ISortHelper<Service> _sortHelper { get; set; }

        public ServiceRepository(MicroBeardContext repositoryContext, ISortHelper<Service> sortHelper)
        {
            _repositoryContext = repositoryContext;
            _sortHelper = sortHelper;
        }

        public PagedList<Service> GetAllServices(ServiceParameters serviceParameters)
        {
            var services = _repositoryContext.Services.AsNoTracking()
                .Where(c => c.Deleted != true);

            SearchByName(ref services, serviceParameters.Name);

            var sortedServices = _sortHelper.ApplySort(services, serviceParameters.OrderBy);

            return PagedList<Service>.ToPagedList(
                sortedServices,
                serviceParameters.PageNumber,
                serviceParameters.PageSize);
        }

        public Service GetServiceByCode(int? code, bool expandRelations = false)
        {
            if (code == null)
                return null;

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

        private void SearchByName(ref IQueryable<Service> services, string serviceName)
        {
            if (!services.Any() || string.IsNullOrWhiteSpace(serviceName))
                return;

            services = services.Where(c => c.Name.ToLower().Contains(serviceName.Trim().ToLower()));
        }
    }
}
