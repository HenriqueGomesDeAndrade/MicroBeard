using MicroBeard.Entities.Models;
namespace MicroBeard.Contracts
{
    public interface IServiceRepository
    {
        IEnumerable<Service> GetAllServices();
        Service GetServiceByCode(int code);
        void CreateService(Service service);
        void UpdateService(Service service);
        void DeleteService(Service service);
    }
}
