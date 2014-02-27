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
        /// <summary>
        /// Quickly open a sqlconnection and if it doesn't respond in the timeout time, throw a ConnectionError with the errorMessage
        /// </summary>
        /// <param name="conn">The sqlconnection that you are about to connect to</param>
        /// <param name="timeout">The timeout time in ms</param>
        /// <param name="errorMessage">The errormessage that the ConnectionException will have</param>
        public static void QuickOpen(this SqlConnection conn, int timeout, string errorMessage = "Timed out while trying to connect.")
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
        /// <summary>
        /// Deletes the contact with the given contactId
        /// </summary>
        /// <param name="contactId">Integer representing a contact</param>
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
        /// <summary>
        /// Get a given contact given a contactId
        /// </summary>
        /// <param name="contactId">Integer representing a contact</param>
        /// <returns></returns>
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
        /// <summary>
        /// Get all contacts
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// GetContactsPageWise is used by the ListView to populate it with pagination
        /// Gets maximumRows amount of contacts given a startRowIndex
        /// Sets totalRowCount based on the query
        /// </summary>
        /// <param name="maximumRows">Amount of rows to get</param>
        /// <param name="startRowIndex">Index to start on.</param>
        /// <param name="totalRowCount"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Creates a new element in the database and put the contact into it
        /// </summary>
        /// <param name="contact"></param>
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
        /// <summary>
        /// Update a already existing contact with the new information
        /// </summary>
        /// <param name="contact"></param>
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