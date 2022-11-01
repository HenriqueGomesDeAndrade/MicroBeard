using MicroBeard.Entities.Models;
using MicroBeard.Entities.Parameters;
using MicroBeard.Helpers;

namespace MicroBeard.Contracts
{
    public interface IContactRepository
    {
        PagedList<Contact> GetAllContacts(ContactParameters contactParameters);
        Contact GetContactByCode(int? code, bool expandRelations = false);
        Contact GetContactByEmail(string email);
        void CreateContact(Contact contact);
        void UpdateContact(Contact contact);
        void DeleteContact(Contact contact);
    }
}
