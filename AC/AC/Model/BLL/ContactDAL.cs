using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;

namespace AC.Model
{
    public static class SqlExtensions
    {
        public static void QuickOpen(this SqlConnection conn, int timeout, string errorMessage)
        {
            // We'll use a Stopwatch here for simplicity. A comparison to a stored DateTime.Now value could also be used
            Stopwatch sw = new Stopwatch();
            bool connectSuccess = false;

            // Try to open the connection, if anything goes wrong, make sure we set connectSuccess = false
            Thread t = new Thread(delegate()
            {
                try
                {
                    sw.Start();
                    conn.Open();
                    connectSuccess = true;
                }
                catch { }
            });

            // Make sure it's marked as a background thread so it'll get cleaned up automatically
            t.IsBackground = true;
            t.Start();

            // Keep trying to join the thread until we either succeed or the timeout value has been exceeded
            while (timeout > sw.ElapsedMilliseconds)
                if (t.Join(1))
                    break;

            // If we didn't connect successfully, throw an exception
            if (!connectSuccess)
                throw new ConnectionException(errorMessage);
        }
    }
    public class ContactDAL : DALBase
    {
        public void DeleteContact(int contactId)
        {

            using (var conn = CreateConnection())
            {
                SqlCommand cmd = new SqlCommand("Person.uspRemoveContact", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ContactID", SqlDbType.Int, 4).Value = contactId;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
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
                    int contactIDindex = reader.GetOrdinal("ContactId");
                    int firstNameIndex = reader.GetOrdinal("FirstName");
                    int lastNameIndex = reader.GetOrdinal("LastName");
                    int emailAdressIndex = reader.GetOrdinal("EmailAddress");

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
            }
        }
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
                    int contactIDindex = reader.GetOrdinal("ContactId");
                    int firstNameIndex = reader.GetOrdinal("FirstName");
                    int lastNameIndex = reader.GetOrdinal("LastName");
                    int emailAdressIndex = reader.GetOrdinal("EmailAddress");

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
            }
        }
        public IEnumerable<Contact> GetContactsPageWise(int maximumRows, int startRowIndex, out int totalRowCount)
        {
            using (var conn = CreateConnection())
            {
                try
                {
                    var contacts = new List<Contact>();
                    var cmd = new SqlCommand("Person.uspGetContactsPageWise", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PageIndex", SqlDbType.Int, 4).Value = startRowIndex / maximumRows +1 ;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int, 4).Value = maximumRows;
                    cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    totalRowCount = (int)cmd.Parameters["@RecordCount"].Value;
                    using (var reader = cmd.ExecuteReader())
                    {
                        var contactIdIndex = reader.GetOrdinal("ContactID");
                        int firstNameIndex = reader.GetOrdinal("FirstName");
                        int lastNameIndex = reader.GetOrdinal("LastName");
                        int emailAdressIndex = reader.GetOrdinal("EmailAddress");

                        while (reader.Read())
                        {
                            contacts.Add(new Contact
                            {
                                ContactId = reader.GetInt32(contactIdIndex),
                                FirstName = reader.GetString(firstNameIndex),
                                LastName = reader.GetString(lastNameIndex),
                                EmailAddress = reader.GetString(emailAdressIndex)
                            });
                        }
                    }
                    return contacts;
                }
                catch
                {
                    throw;
                }
            }
        }
        public void InsertContact(Contact contact)
        {
            using (var conn = CreateConnection())
            {
                SqlCommand cmd = new SqlCommand("Person.uspAddContact", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = contact.FirstName;
                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = contact.LastName;
                cmd.Parameters.Add("@EmailAddress", SqlDbType.NVarChar, 50).Value = contact.EmailAddress;

                cmd.Parameters.Add("@ContactID", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                conn.Open();
                cmd.ExecuteNonQuery();

                contact.ContactId = (int)cmd.Parameters["@ContactID"].Value;
            }
        }
        public void UpdateContact(Contact contact)
        {
            using (var conn = CreateConnection())
            {
                SqlCommand cmd = new SqlCommand("Person.uspUpdateContact", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ContactID", SqlDbType.Int, 4).Value = contact.ContactId;
                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = contact.FirstName;
                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = contact.LastName;
                cmd.Parameters.Add("@EmailAddress", SqlDbType.NVarChar, 50).Value = contact.EmailAddress;

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}