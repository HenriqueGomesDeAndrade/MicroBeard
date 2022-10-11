using MicroBeard.Entities.Models;

namespace MicroBeard.Contracts
{
    public interface IContactRepository : IRepositoryBase<Contact>
    {
        IEnumerable<Contact> GetAllContacts();
        Contact GetContactByCode(int code);
        Contact GetContactWithDetails(int code);
        void CreateContact(Contact contact);
        void UpdateContact(Contact contact);
        void DeleteContact(Contact contact);
    }
}
