using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data.SqlClient;

namespace AC.Model
{
    abstract public class DALBase
    {
        string _connectionString;

        SqlConnection CreateConnection()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                return conn;
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