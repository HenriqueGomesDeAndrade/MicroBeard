using MicroBeard.Entities.Models;
using MicroBeard.Entities.Parameters;
using MicroBeard.Helpers;

namespace MicroBeard.Contracts
{
    public interface IServiceRepository
    {
        PagedList<Service> GetAllServices(ServiceParameters serviceParameters);
        Service GetServiceByCode(int? code, bool expandRelations = false);
        void CreateService(Service service);
        void UpdateService(Service service);
        void DeleteService(Service service);
    }
}
