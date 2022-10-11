using Entities;
using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;
using System.Data.Entity;


namespace MicroBeard.Repository
{
    public class SchedulingRepository : RepositoryBase<Scheduling>, ISchedulingRepository
    {
        public SchedulingRepository(RepositoryContext respositoryContext)
            :base(respositoryContext)
        {
            
        }

        public IEnumerable<Scheduling> GetAllSchedulings()
        {
            return FindAll()
                .Where(c => c.Deleted != true)
                .OrderBy(c => c.Date)
                .ToList();
        }

        public Scheduling GetSchedulingByCode(int code)
        {
            return FindByCondition(c => c.Deleted != true && c.Code.Equals(code)).FirstOrDefault();
        }

        public Scheduling GetSchedulingWithDetails(int code)
        {
            return FindByCondition(c => c.Deleted != true && c.Code.Equals(code)).Include(s => s.Contact).Include(s => s.Service).FirstOrDefault();
        }

        public void CreateScheduling(Scheduling scheduling)
        {
            Create(scheduling);
        }

        public void UpdateScheduling(Scheduling scheduling)
        {
            Update(scheduling);
        }

        public void DeleteScheduling(Scheduling scheduling)
        {
            Delete(scheduling);
        }
    }
}
