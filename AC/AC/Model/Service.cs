using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AC.Model
{
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
        public void DeleteContact(int contactId) 
        {
            ContactDAL.DeleteContact(contactId);
        }
        public Contact GetContact(int contactId) 
        {
            return ContactDAL.GetContactById(contactId);
        }
        public IEnumerable<Contact> GetContacts() 
        {
            return ContactDAL.GetContacts();
        }
        public IEnumerable<Contact> GetContactsPageWise(int maximumRows, int startRowIndex, out int totalRowCount) 
        {
            return ContactDAL.GetContactsPageWise(maximumRows, startRowIndex, out totalRowCount);
        }
        public void SaveContact(Contact contact) 
        {
            ContactDAL.InsertContact(contact);
        }
    }
}