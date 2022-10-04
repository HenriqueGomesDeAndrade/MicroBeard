using Entities;
using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;

namespace MicroBeard.Repository
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(RepositoryContext respositoryContext)
            :base(respositoryContext)
        {
            
        }
    }
}
