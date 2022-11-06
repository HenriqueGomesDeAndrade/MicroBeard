using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;
using MicroBeard.Entities.Parameters;
using MicroBeard.Helpers;
using MicroBeard.Helpers.Sort;
using System.Data.Entity;

namespace MicroBeard.Repository
{
    public class ContactRepository : IContactRepository
    {
        private MicroBeardContext _repositoryContext { get; set; }
        private ISortHelper<Contact> _sortHelper { get; set; }


        public ContactRepository(MicroBeardContext repositoryContext, ISortHelper<Contact> sortHelper)
        {
            _repositoryContext = repositoryContext;
            _sortHelper = sortHelper;
        }

        public PagedList<Contact> GetAllContacts(ContactParameters contactParameters)
        {
            var contacts = _repositoryContext.Contacts.AsNoTracking()
                .Where(c => c.Deleted != true);

            SearchByName(ref contacts, contactParameters.Name);
            SearchByCpf(ref contacts, contactParameters.Cpf);
            SearchByEmail(ref contacts, contactParameters.Email);

            var sortedContacts = _sortHelper.ApplySort(contacts, contactParameters.OrderBy);

            return PagedList<Contact>.ToPagedList(
                sortedContacts,
                contactParameters.PageNumber,
                contactParameters.PageSize);
        }

        public Contact GetContactByCode(int? code, bool expandRelations = false)
        {
            if (code == null)
                return null;

            Contact contact = _repositoryContext.Contacts.Where(c => c.Deleted != true && c.Code.Equals(code)).FirstOrDefault();

            if (contact != null && expandRelations)
            {
                _repositoryContext.Entry(contact).Collection(c => c.Schedulings).Load();
                if (contact.Schedulings != null)
                    foreach (var scheduling in contact.Schedulings)
                        if (scheduling.Deleted == true)
                            contact.Schedulings.Remove(scheduling);
            }
               
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

        private void SearchByName(ref IQueryable<Contact> contacts, string contactName)
        {
            if (!contacts.Any() || string.IsNullOrWhiteSpace(contactName))
                return;

            contacts = contacts.Where(c => c.Name.ToLower().Contains(contactName.Trim().ToLower()));
        }

        private void SearchByCpf(ref IQueryable<Contact> contacts, string contactCpf)
        {
            if (!contacts.Any() || string.IsNullOrWhiteSpace(contactCpf))
                return;

            contacts = contacts.Where(c => c.Cpf.Contains(contactCpf));
        }

        private void SearchByEmail(ref IQueryable<Contact> contacts, string contactEmail)
        {
            if (!contacts.Any() || string.IsNullOrWhiteSpace(contactEmail))
                return;

            contacts = contacts.Where(c => c.Email.ToLower().Contains(contactEmail.ToLower()));
        }
    }
}
