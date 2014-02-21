using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AC.Model
{
    public class ContactDAL : DALBase
    {
        public void DeleteContact()
        {

        }
        public Contact GetContactById(int contactId)
        {
            return new Contact();
        }
        public IEnumerable<Contact> GetContacts()
        {
            return new List<Contact>();
        }
        public IEnumerable<Contact> GetContactsPageWise(int maximumRows, int startRowIndex, out int totalRowCount)
        {
            totalRowCount = 1;
            return new List<Contact>();
        }
        public void InsertContact(Contact contact)
        {

        }
        public void UpdateContact(Contact contact)
        {

        }
    }
}