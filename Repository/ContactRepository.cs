using Entities;
using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;
using System.Data.Entity;

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
            return FindByCondition(c => c.Code.Equals(code)).FirstOrDefault();
        }

        public Contact GetContactWithDetails(int code)
        {
            return FindByCondition(c => c.Code.Equals(code)).Include(sc => sc.Schedulings).FirstOrDefault();
        }

        public void CreateContact(Contact contact)
        {
            Create(contact);
        }

        public void UpdateContact(Contact contact)
        {
            Update(contact);
        }

        public void DeleteContact(Contact contact)
        {
            Delete(contact);
        }
    }
}
