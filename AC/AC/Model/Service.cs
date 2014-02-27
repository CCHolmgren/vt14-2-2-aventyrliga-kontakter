using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AC.Model
{
    /// <summary>
    /// Service handles all database handling
    /// Service is the only code that should call the DAL code directly
    /// </summary>
    public class Service
    {
        ContactDAL _contactDAL;
        public ContactDAL ContactDAL
        {
            get
            {
                return _contactDAL ?? (_contactDAL = new ContactDAL());
            }
        }
        /// <summary>
        /// Deletes the contact represented by the contactId
        /// </summary>
        /// <param name="contactId"></param>
        public void DeleteContact(int contactId) 
        {
            ContactDAL.DeleteContact(contactId);
        }
        /// <summary>
        /// Gets the contact represented by the contactId
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public Contact GetContact(int contactId) 
        {
            return ContactDAL.GetContactById(contactId);
        }
        /// <summary>
        /// Get all contacts
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Contact> GetContacts() 
        {
            return ContactDAL.GetContacts();
        }
        /// <summary>
        /// GetContactsPageWise is used by the ListView to populate it with pagination
        /// Gets maximumRows amount of contacts given a startRowIndex
        /// Sets totalRowCount based on the query
        /// </summary>
        /// <param name="maximumRows">Amount of rows to get</param>
        /// <param name="startRowIndex">Index to start on.</param>
        /// <param name="totalRowCount"></param>
        /// <returns>List with maximumRows amount of contacts</returns>
        public IEnumerable<Contact> GetContactsPageWise(int maximumRows, int startRowIndex, out int totalRowCount) 
        {
            return ContactDAL.GetContactsPageWise(maximumRows, startRowIndex, out totalRowCount);
        }
        /// <summary>
        /// Save the contact. If ContactId is 0 we have a new contact so Insert a new one
        /// Otherwise we have an old contact, so we can update it
        /// </summary>
        /// <param name="contact"></param>
        public void SaveContact(Contact contact) 
        {
            ICollection<ValidationResult> validationResult;
            if (!contact.Validate(out validationResult))
            {
                var vx = new ValidationException("Objektet klarade inte valideringen.");
                vx.Data.Add("validationResult", validationResult);
                throw vx;
            }
            if (contact.ContactId == 0)
            {
                ContactDAL.InsertContact(contact);
            }
            else
            {
                ContactDAL.UpdateContact(contact);
            }
        }
    }
}