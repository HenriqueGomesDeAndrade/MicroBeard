using Entities;
using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;

namespace MicroBeard.Repository
{
    public class CollaboratorRepository : RepositoryBase<Collaborator>, ICollaboratorRepository
    {
        public CollaboratorRepository(RepositoryContext respositoryContext)
            :base(respositoryContext)
        {
            
        }
    }
}
