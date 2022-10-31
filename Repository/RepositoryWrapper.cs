using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;
using MicroBeard.Helpers.Sort;

namespace MicroBeard.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private MicroBeardContext _repositoryContext;
        private IContactRepository _contact;
        private ICollaboratorRepository _collaborator;
        private ILicenseRepository _license;
        private ISchedulingRepository _scheduling;
        private IServiceRepository _service;

        private ISortHelper<Contact> _contactSortHelper;
        private ISortHelper<Collaborator> _collaboratorSortHelper;
        private ISortHelper<License> _licenseSortHelper;
        private ISortHelper<Scheduling> _schedulingSortHelper;
        private ISortHelper<Service> _serviceSortHelper;


        public RepositoryWrapper(MicroBeardContext repositoryContext,
            ISortHelper<Contact> contactSortHelper,
            ISortHelper<Collaborator> collaboratorSortHelper,
            ISortHelper<License> licenseSortHelper,
            ISortHelper<Scheduling> schedulingSortHelper,
            ISortHelper<Service> serviceSortHelper)
        {
            _repositoryContext = repositoryContext;
            _contactSortHelper = contactSortHelper;
            _collaboratorSortHelper = collaboratorSortHelper;
            _licenseSortHelper = licenseSortHelper;
            _schedulingSortHelper = schedulingSortHelper;
            _serviceSortHelper = serviceSortHelper;
        }

        public IContactRepository Contact
        {
            get
            {
                if (_contact == null)
                    _contact = new ContactRepository(_repositoryContext, _contactSortHelper);

                return _contact;
            }
        }

        public ICollaboratorRepository Collaborator
        {
            get
            {
                if (_collaborator == null)
                    _collaborator = new CollaboratorRepository(_repositoryContext, _collaboratorSortHelper);

                return _collaborator;
            }
        }

        public ILicenseRepository License
        {
            get
            {
                if (_license == null)
                    _license = new LicenseRepository(_repositoryContext, _licenseSortHelper);

                return _license;
            }
        }

        public ISchedulingRepository Scheduling
        {
            get
            {
                if (_scheduling == null)
                    _scheduling = new SchedulingRepository(_repositoryContext, _schedulingSortHelper);

                return _scheduling;
            }
        }

        public IServiceRepository Service
        {
            get
            {
                if (_service == null)
                    _service = new ServiceRepository(_repositoryContext, _serviceSortHelper);

                return _service;
            }
        }

        public void Save()
        {
            _repositoryContext.SaveChanges();
        }

        public void UnchangeProperty(object target, string propertyName)
        {
            var isModified = _repositoryContext.Entry(target).Property(propertyName).IsModified;
            if (isModified)
                _repositoryContext.Entry(target).Property(propertyName).IsModified = false;
        }

    }
}
