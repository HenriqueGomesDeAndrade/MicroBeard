using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;
using System.Data.Entity;

namespace MicroBeard.Repository
{
    public class ContactRepository : IContactRepository
    {
        private MicroBeardContext _repositoryContext { get; set; }

        public ContactRepository(MicroBeardContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IEnumerable<Contact> GetAllContacts()
        {
            return _repositoryContext.Contacts.AsNoTracking()
                .Where(c => c.Deleted != true)
                .OrderBy(c => c.Name)
                .ToList();
        }

        public Contact GetContactByCode(int code)
        {
            Contact contact = _repositoryContext.Contacts.AsNoTracking().Where(c => c.Deleted != true && c.Code.Equals(code)).FirstOrDefault();

            _repositoryContext.Entry(contact).Collection(c => c.Schedulings).Load();

            return contact;
        }

        public void CreateContact(Contact contact)
        {
            _repositoryContext.Contacts.Add(contact);
        }

        public void UpdateContact(Contact contact)
        {
            _repositoryContext.Contacts.Update(contact);
        }

        public void DeleteContact(Contact contact)
        {
            _repositoryContext.Contacts.Remove(contact);
        }
    }
}
