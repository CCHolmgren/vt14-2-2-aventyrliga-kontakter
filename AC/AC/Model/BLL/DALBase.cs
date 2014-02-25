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
            SqlExtensions.QuickOpen(new SqlConnection(_connectionString), 5000, "Anslutningen till databasen misslyckades.");
            return new SqlConnection(_connectionString);
        }
        public DALBase()
        {
            _connectionString = WebConfigurationManager.ConnectionStrings["ApplicationService"].ConnectionString;
        }
    }
}