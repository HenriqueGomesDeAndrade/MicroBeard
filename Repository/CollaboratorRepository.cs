using Entities;
using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;
using System.Data.Entity;

namespace MicroBeard.Repository
{
    public class CollaboratorRepository : RepositoryBase<Collaborator>, ICollaboratorRepository
    {
        public CollaboratorRepository(RepositoryContext respositoryContext)
            :base(respositoryContext)
        {
            
        }

        public IEnumerable<Collaborator> GetAllCollaborators()
        {
            return FindAll()
                .Where(c => c.Desactivated != true)
                .OrderBy(c => c.Name)
                .ToList();
        }

        public Collaborator GetCollaboratorByCode(int code)
        {
            return FindByCondition(c => c.Desactivated != true && c.Code.Equals(code)).FirstOrDefault();
        }

        public Collaborator GetCollaboratorWithDetails(int code)
        {
            return FindByCondition(c => c.Desactivated != true && c.Code.Equals(code)).Include(c => c.Services).Include((c => c.Licenses)).FirstOrDefault();
        }

        public void CreateCollaborator(Collaborator collaborator)
        {
            Create(collaborator);
        }

        public void UpdateCollaborator(Collaborator collaborator)
        {
            Update(collaborator);
        }

        public void DeleteCollaborator(Collaborator collaborator)
        {
            Delete(collaborator);
        }
    }
}
