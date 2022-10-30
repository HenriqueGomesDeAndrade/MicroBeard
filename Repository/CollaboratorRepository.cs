using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;
using MicroBeard.Entities.Parameters;
using MicroBeard.Helpers;
using System.Data.Entity;

namespace MicroBeard.Repository
{
    public class CollaboratorRepository :  ICollaboratorRepository
    {
        private MicroBeardContext _repositoryContext { get; set; }

        public CollaboratorRepository(MicroBeardContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public PagedList<Collaborator> GetAllCollaborators(CollaboratorParameters collaboratorParameters)
        {
            var collaborators = _repositoryContext.Collaborators.AsNoTracking()
                .Where(c => c.Desactivated != true);

            SearchByName(ref collaborators, collaboratorParameters.Name);
            SearchByCpf(ref collaborators, collaboratorParameters.Cpf);
            SearchByEmail(ref collaborators, collaboratorParameters.Email);

            return PagedList<Collaborator>.ToPagedList(
                collaborators.OrderBy(c => c.Name),
                collaboratorParameters.PageNumber,
                collaboratorParameters.PageSize);
        }

        public Collaborator GetCollaboratorByCode(int code, bool expandRelations = false)
        {
            Collaborator collaborator = _repositoryContext.Collaborators.AsNoTracking().Where(c => c.Desactivated != true && c.Code.Equals(code)).FirstOrDefault();

            if (collaborator != null && expandRelations)
            {
                _repositoryContext.Entry(collaborator).Collection(c => c.Licenses).Load();
                _repositoryContext.Entry(collaborator).Collection(c => c.Services).Load();
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
