using ContactManager.Models;

namespace ContactManager.Data
{
    public interface IContactRepository : IDisposable
    {
        IEnumerable<Contact> GetContacts();
        Contact? GetContactByID(int contactId);
        void InsertContact(Contact contact);
        void DeleteContact(int contactID);
        void UpdateContact(Contact contact);
        void Save();
    }
}
