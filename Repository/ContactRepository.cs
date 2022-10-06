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

        public IEnumerable<Contact> GetAllContacts()
        {
            return FindAll()
                .OrderBy(c => c.Name)
                .ToList();
        }

        public Contact GetContactByCode(int code)
        {
            return FindByCollecion(c => c.Code == code).FirstOrDefault();
        }
    }
}
