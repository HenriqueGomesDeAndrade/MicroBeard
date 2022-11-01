using MicroBeard.Entities.Models;
using MicroBeard.Entities.Parameters;
using MicroBeard.Helpers;

namespace MicroBeard.Contracts
{
    public interface ISchedulingRepository
    {
        PagedList<Scheduling> GetAllSchedulings(SchedulingParameters schedullingParameters);
        Scheduling GetSchedulingByCode(int? code, bool expandRelations = false);
        void CreateScheduling(Scheduling Scheduling);
        void UpdateScheduling(Scheduling Scheduling);
        void DeleteScheduling(Scheduling Scheduling);
    }
}
