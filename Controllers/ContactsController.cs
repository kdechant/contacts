using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactManager.Data;
using ContactManager.Models;

namespace ContactManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactRepository _repository;

        public ContactsController(IContactRepository repository) => _repository = repository;

        // GET: api/Contacts
        [HttpGet]
        public ActionResult<IEnumerable<Contact>> GetContacts()
        {
            return _repository.GetContacts().ToArray();
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public ActionResult<Contact> GetContact(int id)
        {
            var contact = _repository.GetContactByID(id);

            if (contact == null)
            {
                return NotFound();
            }

            return contact;
        }

        // PUT: api/Contacts/5
        [HttpPut("{id}")]
        public IActionResult PutContact(int id, Contact contact)
        {
            if (id != contact.Id)
            {
                // Catches user mischief. Can't change the ID of a row.
                return BadRequest();
            }

            _repository.UpdateContact(contact);

            try
            {
                _repository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                // UPDATE query failed. Check if the row actually exists.
                if (_repository.GetContactByID(id) == null)
                {
                    // Tried to update a row that doesn't exist.
                    return NotFound();
                }
                else
                {
                    // Something else went wrong
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Contacts
        [HttpPost]
        public ActionResult<Contact> PostContact(Contact contact)
        {
            _repository.InsertContact(contact);
            _repository.Save();

            // A fancier app would catch exceptions here for errors like trying to 
            // insert a duplicate contact. But this sample app doesn't have any
            // validation, so the INSERT query will always succeed.

            return CreatedAtAction("GetContact", new { id = contact.Id }, contact);
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public IActionResult DeleteContact(int id)
        {
            var contact = _repository.GetContactByID(id);
            if (contact == null)
            {
                return NotFound();
            }

            _repository.DeleteContact(id);

            return NoContent();
        }

        private bool ContactExists(int id)
        {
            return _repository.GetContactByID(id) != null;
        }
    }
}
