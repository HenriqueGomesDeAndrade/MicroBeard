using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;

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

        public RepositoryWrapper(MicroBeardContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IContactRepository Contact
        {
            get
            {
                if (_contact == null)
                    _contact = new ContactRepository(_repositoryContext);

                return _contact;
            }
        }

        public ICollaboratorRepository Collaborator
        {
            get
            {
                if (_collaborator == null)
                    _collaborator = new CollaboratorRepository(_repositoryContext);

                return _collaborator;
            }
        }

        public ILicenseRepository License
        {
            get
            {
                if (_license == null)
                    _license = new LicenseRepository(_repositoryContext);

                return _license;
            }
        }

        public ISchedulingRepository Scheduling
        {
            get
            {
                if (_scheduling == null)
                    _scheduling = new SchedulingRepository(_repositoryContext);

                return _scheduling;
            }
        }

        public IServiceRepository Service
        {
            get
            {
                if (_service == null)
                    _service = new ServiceRepository(_repositoryContext);

                return _service;
            }
        }

        public void Save()
        {
            _repositoryContext.SaveChanges();
        }
    }
}
