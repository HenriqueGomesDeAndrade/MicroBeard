using AutoMapper.EntityFrameworkCore;
using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;
using MicroBeard.Entities.Parameters;
using MicroBeard.Helpers;
using MicroBeard.Helpers.Sort;
using System.Data.Entity;

namespace MicroBeard.Repository
{
    public class CollaboratorRepository :  ICollaboratorRepository
    {
        private MicroBeardContext _repositoryContext { get; set; }
        private ISortHelper<Collaborator> _sortHelper { get; set; }

        public CollaboratorRepository(MicroBeardContext repositoryContext, ISortHelper<Collaborator> sortHelper)
        {
            _repositoryContext = repositoryContext;
            _sortHelper = sortHelper;
        }

        public PagedList<Collaborator> GetAllCollaborators(CollaboratorParameters collaboratorParameters)
        {
            var collaborators = _repositoryContext.Collaborators.AsNoTracking()
                .Where(c => c.Desactivated != true);

            SearchByName(ref collaborators, collaboratorParameters.Name);
            SearchByCpf(ref collaborators, collaboratorParameters.Cpf);
            SearchByEmail(ref collaborators, collaboratorParameters.Email);

            var sortedCollaborators = _sortHelper.ApplySort(collaborators, collaboratorParameters.OrderBy);

            return PagedList<Collaborator>.ToPagedList(
                sortedCollaborators,
                collaboratorParameters.PageNumber,
                collaboratorParameters.PageSize);
        }

        public Collaborator GetCollaboratorByCode(int? code, bool expandRelations = false)
        {
            if (code == null)
                return null;

            Collaborator collaborator = _repositoryContext.Collaborators.Where(c => c.Desactivated != true && c.Code.Equals(code)).FirstOrDefault();

            if (collaborator != null && expandRelations)
            {
                _repositoryContext.Entry(collaborator).Collection(c => c.Licenses).Load();
                if(collaborator.Licenses != null)
                    foreach(var license in collaborator.Licenses)
                        if(license.Desactivated == true)
                            collaborator.Licenses.Remove(license);
                    
                _repositoryContext.Entry(collaborator).Collection(c => c.Services).Load();
                if (collaborator.Services != null)
                    foreach (var service in collaborator.Services)
                        if (service.Deleted == true)
                            collaborator.Services.Remove(service);

                _repositoryContext.Entry(collaborator).Collection(c => c.Schedulings).Load();
                if (collaborator.Schedulings != null)
                    foreach (var scheduling in collaborator.Schedulings)
                        if (scheduling.Deleted == true)
                            collaborator.Schedulings.Remove(scheduling);
            }

            return collaborator;
        }

        public Collaborator GetCollaboratorByEmail(string email)
        {
            return _repositoryContext.Collaborators.AsNoTracking().Where(c => c.Desactivated != true && c.Email.Equals(email)).FirstOrDefault();
        }

        public void CreateCollaborator(Collaborator collaborator)
        {
            _repositoryContext.Collaborators.Add(collaborator);
        }

        public void UpdateCollaborator(Collaborator collaborator)
        {
            _repositoryContext.Collaborators.Update(collaborator);
        }

        public void DeleteCollaborator(Collaborator collaborator)
        {
            _repositoryContext.Collaborators.Remove(collaborator);
        }

        public bool CheckIfIsLastAdminCollaborator(int collaboratorCode)
        {
            bool isLasAdminCollaborator = false;
            var lastCollaborator = _repositoryContext.Collaborators.AsNoTracking().FirstOrDefault(c => c.IsAdmin == true && c.Code != collaboratorCode);
                if(lastCollaborator == null)
                    isLasAdminCollaborator = true;
            return isLasAdminCollaborator;
        }

        private void SearchByName(ref IQueryable<Collaborator> collaborators, string collaboratorName)
        {
            if (!collaborators.Any() || string.IsNullOrWhiteSpace(collaboratorName))
                return;

            collaborators = collaborators.Where(c => c.Name.ToLower().Contains(collaboratorName.Trim().ToLower()));
        }

        private void SearchByCpf(ref IQueryable<Collaborator> collaborators, string collaboratorCpf)
        {
            if (!collaborators.Any() || string.IsNullOrWhiteSpace(collaboratorCpf))
                return;

            collaborators = collaborators.Where(c => c.Cpf.Contains(collaboratorCpf));
        }

        private void SearchByEmail(ref IQueryable<Collaborator> collaborators, string collaboratorEmail)    
        {
            if (!collaborators.Any() || string.IsNullOrWhiteSpace(collaboratorEmail))
                return;

            collaborators = collaborators.Where(c => c.Email.ToLower().Contains(collaboratorEmail.ToLower()));
        }
    }
}
