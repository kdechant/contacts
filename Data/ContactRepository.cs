using ContactManager.Models;

namespace ContactManager.Data
{
    public class ContactRepository : IContactRepository
    {
        private DataContext _context;

        public ContactRepository(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Contact> GetContacts()
        {
            return _context.Contacts;
        }

        public Contact? GetContactByID(int id)
        {
            var contact = _context.Contacts.Find(id);
            return contact;
        }

        public void InsertContact(Contact contact)
        {
            _context.Contacts.Add(contact);
        }

        public void UpdateContact(Contact contact)
        {
            _context.Entry(contact).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void DeleteContact(int id)
        {
            var contact = _context.Contacts.Find(id);
            if (contact == null)
            {
                throw new Exception("Contact not found");
            }
            _context.Contacts.Remove(contact);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
