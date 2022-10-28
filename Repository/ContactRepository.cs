using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;
using MicroBeard.Entities.Parameters;
using MicroBeard.Helpers;
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

        public PagedList<Contact> GetAllContacts(ContactParameters contactParameters)
        {
            return PagedList<Contact>.ToPagedList(
                _repositoryContext.Contacts.AsNoTracking()
                .Where(c => c.Deleted != true)
                .OrderBy(c => c.Name),
                contactParameters.PageNumber,
                contactParameters.PageSize);
        }

        public Contact GetContactByCode(int code, bool expandRelations = false)
        {
            Contact contact = _repositoryContext.Contacts.AsNoTracking().Where(c => c.Deleted != true && c.Code.Equals(code)).FirstOrDefault();

            if (contact != null && expandRelations)
                _repositoryContext.Entry(contact).Collection(c => c.Schedulings).Load();

            return contact;
        }

        public Contact GetContactByEmail(string email)
        {
            return _repositoryContext.Contacts.AsNoTracking().Where(c => c.Deleted != true && c.Email.Equals(email)).FirstOrDefault();
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
