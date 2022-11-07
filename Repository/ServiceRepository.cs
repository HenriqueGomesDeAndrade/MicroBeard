using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;
using MicroBeard.Entities.Parameters;
using MicroBeard.Helpers;
using MicroBeard.Helpers.Sort;
using System.Collections;
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
            var services = _repositoryContext.Services.AsNoTracking().Include(c => c.Collaborators)
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
                if (service.Schedulings != null)
                    foreach (var scheduling in service.Schedulings)
                        if (scheduling.Deleted == true)
                            service.Schedulings.Remove(scheduling);

                _repositoryContext.Entry(service).Collection(c => c.Collaborators).Load();
                if (service.Collaborators != null)
                    foreach (var collaborator in service.Collaborators)
                        if (collaborator.Desactivated == true)
                            service.Collaborators.Remove(collaborator);

                _repositoryContext.Entry(service).Reference(c => c.LicenseCodeNavigation).Load();
                if (service.LicenseCodeNavigation != null)
                    if (service.LicenseCodeNavigation.Desactivated == true)
                        service.LicenseCodeNavigation = null;

                _repositoryContext.Attach(service);


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
