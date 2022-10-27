using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;
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

        public IEnumerable<Collaborator> GetAllCollaborators()
        {
            return _repositoryContext.Collaborators.AsNoTracking()
                .Where(c => c.Desactivated != true)
                .OrderBy(c => c.Name)
                .ToList();
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
    }
}
