using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data.SqlClient;

namespace AC.Model
{
    [Serializable]
    public class ConnectionException : Exception
    {
        public ConnectionException() { }
        public ConnectionException(string message) : base(message) { }
        public ConnectionException(string message, Exception inner) : base(message, inner) { }
        protected ConnectionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
    abstract public class DALBase
    {
        string _connectionString;

        protected SqlConnection CreateConnection()
        {
            //Attempts to connect with the QuickOpen function which throws after 5000 ms if we didn't connect
            //That way we can assume that it's safe to connect to the server and we won't get 30 s of loading
            try
            {
                SqlExtensions.QuickOpen(new SqlConnection(_connectionString), 5000);
                return new SqlConnection(_connectionString);
            }
            catch
            {
                throw new ConnectionException("Anslutningen till servern misslyckades.");
            }
        }
        public DALBase()
        {
            _connectionString = WebConfigurationManager.ConnectionStrings["ApplicationService"].ConnectionString;
        }
        /*static string connectionString = WebConfigurationManager.ConnectionStrings["ApplicationService"].ConnectionString;
        public static string what()
        {
            string result = "";
            using(var conn = new SqlConnection(connectionString))
            {
                try
                {
                    var cmd = new SqlCommand("Person.uspGetContacts", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result += reader.GetInt32(0);
                            result += reader.GetString(1);
                            result += "\n";
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }
            return result;
        }*/
    }
}