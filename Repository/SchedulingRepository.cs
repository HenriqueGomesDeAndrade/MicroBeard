using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;
using MicroBeard.Entities.Parameters;
using MicroBeard.Helpers;
using System.ComponentModel;
using System.Data.Entity;


namespace MicroBeard.Repository
{
    public class SchedulingRepository :  ISchedulingRepository
    {
        private MicroBeardContext _repositoryContext { get; set; }

        public SchedulingRepository(MicroBeardContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public PagedList<Scheduling> GetAllSchedulings(SchedulingParameters schedulingParameters)
        {
            return PagedList<Scheduling>.ToPagedList(
                _repositoryContext.Schedulings.AsNoTracking()
                .Where(c => c.Deleted != true)
                .OrderBy(c => c.Date),
                schedulingParameters.PageNumber,
                schedulingParameters.PageSize);
        }

        public Scheduling GetSchedulingByCode(int code, bool expandRelations = false)
        {
            Scheduling scheduling = _repositoryContext.Schedulings.AsNoTracking().Where(c => c.Deleted != true && c.Code.Equals(code)).FirstOrDefault();

            if(scheduling != null && expandRelations)
            {
                _repositoryContext.Entry(scheduling).Reference(c => c.ServiceCodeNavigation).Load();
                _repositoryContext.Entry(scheduling).Reference(c => c.ContactCodeNavigation).Load();
            }

            return scheduling;
        }

        public void CreateScheduling(Scheduling scheduling)
        {
            _repositoryContext.Schedulings.Add(scheduling);
        }

        public void UpdateScheduling(Scheduling scheduling)
        {
            _repositoryContext.Schedulings.Update(scheduling);
        }

        public void DeleteScheduling(Scheduling scheduling)
        {
            _repositoryContext.Schedulings.Remove(scheduling);
        }
    }
}
