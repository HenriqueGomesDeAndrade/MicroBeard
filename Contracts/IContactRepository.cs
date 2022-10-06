using MicroBeard.Entities.Models;
namespace MicroBeard.Contracts
{
    public interface IContactRepository : IRepositoryBase<Contact>
    {
        IEnumerable<Contact> GetAllContacts();
        Contact GetContactByCode(int Code);
    }
}
