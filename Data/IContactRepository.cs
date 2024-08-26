using ContactManager.Models;

namespace ContactManager.Data
{
    public interface IContactRepository : IDisposable
    {
        Task<IEnumerable<Contact>> GetContactsAsync();
        Task<Contact?> GetContactByIdAsync(int id);
        void InsertContact(Contact contact);
        Task DeleteContactAsync(int id);
        void UpdateContact(Contact contact);
        Task SaveAsync();
    }
}
