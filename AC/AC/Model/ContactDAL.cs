using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            using (var conn = CreateConnection())
            {
                var cmd = new SqlCommand("Person.uspGetContact", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ContactId", SqlDbType.Int, 4).Value = contactId;

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    int contactIDindex = reader.GetOrdinal("@ContactId");
                    int firstNameIndex = reader.GetOrdinal("@FirstName");
                    int lastNameIndex = reader.GetOrdinal("@LastName");
                    int emailAdressIndex = reader.GetOrdinal("@EmailAddress");

                    if (reader.Read())
                    {
                        return new Contact
                        {
                            ContactId = reader.GetInt32(contactIDindex),
                            FirstName = reader.GetString(firstNameIndex),
                            LastName = reader.GetString(lastNameIndex),
                            EmailAddress = reader.GetString(emailAdressIndex)
                        };
                    }
                }
            return null;
        }}
        public IEnumerable<Contact> GetContacts()
        {
            using (var conn = CreateConnection())
            {
                List<Contact> contacts = new List<Contact>();

                var cmd = new SqlCommand("Person.uspGetContacts", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                
                using (var reader = cmd.ExecuteReader())
                {
                    int contactIDindex = reader.GetOrdinal("@ContactId");
                    int firstNameIndex = reader.GetOrdinal("@FirstName");
                    int lastNameIndex = reader.GetOrdinal("@LastName");
                    int emailAdressIndex = reader.GetOrdinal("@EmailAddress");

                    while(reader.Read())
                    {
                        contacts.Add(new Contact
                        {
                            ContactId = reader.GetInt32(contactIDindex),
                            FirstName = reader.GetString(firstNameIndex),
                            LastName = reader.GetString(lastNameIndex),
                            EmailAddress = reader.GetString(emailAdressIndex)
                        });
                    }
                }
            return null;
        }}
        public IEnumerable<Contact> GetContactsPageWise(int maximumRows, int startRowIndex, out int totalRowCount)
        {
            using (var conn = CreateConnection())
            {
                var cmd = new SqlCommand("Person.uspGetCOntactsPageWise", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@PageIndex", SqlDbType.Int, 4).Value = startRowIndex;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int, 4).Value = maximumRows;
                cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                conn.Open();
                totalRowCount = (int)cmd.Parameters["@RecordCount"].Value;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                    }
                }
            }
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