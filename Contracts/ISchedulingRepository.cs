using MicroBeard.Entities.Models;


namespace MicroBeard.Contracts
{
    public interface ISchedulingRepository : IRepositoryBase<Scheduling>
    {
        IEnumerable<Scheduling> GetAllSchedulings();
        Scheduling GetSchedulingByCode(int code);
        Scheduling GetSchedulingWithDetails(int code);
        void CreateScheduling(Scheduling Scheduling);
        void UpdateScheduling(Scheduling Scheduling);
        void DeleteScheduling(Scheduling Scheduling);
    }
}
