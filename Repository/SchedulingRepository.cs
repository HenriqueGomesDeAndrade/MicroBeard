﻿using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;
using MicroBeard.Entities.Parameters;
using MicroBeard.Helpers;
using MicroBeard.Helpers.Sort;
using System.ComponentModel;
using System.Data.Entity;


namespace MicroBeard.Repository
{
    public class SchedulingRepository :  ISchedulingRepository
    {
        private MicroBeardContext _repositoryContext { get; set; }
        private ISortHelper<Scheduling> _sortHelper { get; set; }

        public SchedulingRepository(MicroBeardContext repositoryContext, ISortHelper<Scheduling> sortHelper)
        {
            _repositoryContext = repositoryContext;
            _sortHelper = sortHelper;
        }

        public PagedList<Scheduling> GetAllSchedulings(SchedulingParameters schedulingParameters)
        {
            var schedulings = _repositoryContext.Schedulings.AsNoTracking()
                .Where(c => c.Deleted != true);

            SearchByContactCode(ref schedulings, schedulingParameters.ContactCode);
            SearchByServiceCode(ref schedulings, schedulingParameters.ServiceCode);
            SearchByDate(ref schedulings, schedulingParameters);

            var sortedSchedulings = _sortHelper.ApplySort(schedulings, schedulingParameters.OrderBy);

            return PagedList<Scheduling>.ToPagedList(
                sortedSchedulings,
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

        private void SearchByContactCode(ref IQueryable<Scheduling> schedulings, int? schedulingContactCode)
        {
            if (!schedulings.Any() || schedulingContactCode == null || schedulingContactCode == 0)
                return;

            schedulings = schedulings.Where(s => s.ContactCode == schedulingContactCode);
        }

        private void SearchByServiceCode(ref IQueryable<Scheduling> schedulings, int? schedulingServiceCode)
        {
            if (!schedulings.Any() || schedulingServiceCode == null || schedulingServiceCode == 0)
                return;

            schedulings = schedulings.Where(s => s.ServiceCode == schedulingServiceCode);
        }

        private void SearchByDate(ref IQueryable<Scheduling> schedulings, SchedulingParameters schedulingParameters)
        {
            int? year = schedulingParameters.DateYear;
            int? month = schedulingParameters.DateMonth;
            int? day = schedulingParameters.DateDay;

            if (!schedulings.Any())
                return;

            if(year != null)
                schedulings = schedulings.Where(s => s.Date.Year == year);
            if(month != null)
                schedulings = schedulings.Where(s => s.Date.Month == month);
            if(day != null)
                schedulings = schedulings.Where(s => s.Date.Day == day);
        }
    }
}
