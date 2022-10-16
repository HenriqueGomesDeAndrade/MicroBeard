using MicroBeard.Entities.Models;

namespace MicroBeard.Contracts
{
    public interface IContactRepository
    {
        IEnumerable<Contact> GetAllContacts();
        Contact GetContactByCode(int code, bool expandRelations = false);
        void CreateContact(Contact contact);
        void UpdateContact(Contact contact);
        void DeleteContact(Contact contact);
    }
}
