using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AC.Model
{
    public class Service
    {
        ContactDAL _contactDAL;
        ContactDAL ContactDAL { get; }
        void DeleteContact(Contact contact) { }
        void DeleteContact(int contactId) { }
        Contact GetContact(int contactId) { }
        IEnumerable<Contact> GetContacts() { }
        IEnumerable<Contact> GetContactsPageWise(int maximumRows, int startRowIndex, out int totalRowCount) { }
        void SaveContact(Contact contact) { }
    }
}