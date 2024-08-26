using ContactManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Data
{
    public class ContactRepository : IContactRepository
    {
        private DataContext _context;

        public ContactRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contact>> GetContactsAsync()
        {
            return await _context.Contacts.ToListAsync();
        }

        public async Task<Contact?> GetContactByIdAsync(int id)
        {
            return await _context.Contacts.FindAsync(id);
        }

        public void InsertContact(Contact contact)
        {
            _context.Contacts.Add(contact);
        }

        public void UpdateContact(Contact contact)
        {
            _context.Entry(contact).State = EntityState.Modified;
        }

        public async Task DeleteContactAsync(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                throw new KeyNotFoundException("Contact not found");
            }
            _context.Contacts.Remove(contact);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
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
