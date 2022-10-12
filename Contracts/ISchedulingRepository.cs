using MicroBeard.Entities.Models;


namespace MicroBeard.Contracts
{
    public interface ISchedulingRepository
    {
        IEnumerable<Scheduling> GetAllSchedulings();
        Scheduling GetSchedulingByCode(int code);
        void CreateScheduling(Scheduling Scheduling);
        void UpdateScheduling(Scheduling Scheduling);
        void DeleteScheduling(Scheduling Scheduling);
    }
}
