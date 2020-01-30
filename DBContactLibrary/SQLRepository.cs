using DBContactLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContactLibrary
{
    public class SQLRepository : IDisposable
    {
        static SqlConnection sqlConnection = new SqlConnection();

        public SQLRepository()
        {
            sqlConnection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DBContact;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        public void Dispose()
        {
            sqlConnection.Dispose();
        }

        public int CreatePost(string ssn, string firstName, string lastName, string email) 
        {
            int output;
            sqlConnection.Open();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection  = sqlConnection;
                sqlCommand.CommandText = $"CreateContact";
                sqlCommand.CommandType = CommandType.StoredProcedure;

                AddParameterToSQLContact("@ssn",ssn, 16, sqlCommand);
                AddParameterToSQLContact("@firstName", firstName, 64, sqlCommand);
                AddParameterToSQLContact("@lastName", lastName, 64, sqlCommand);
                AddParameterToSQLContact("@email", email, 64, sqlCommand);

                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@ID";
                parameter.SqlDbType = SqlDbType.Int;
                parameter.Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add(parameter);
                sqlCommand.ExecuteNonQuery();
                    
              
                output = (int)sqlCommand.Parameters["@ID"].Value;
            }
            sqlConnection.Close();
            return output;
        }

        public Contact ReadContact(int id)
        {
 
            Contact output = null;

            sqlConnection.Open();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = $"select * from Contact where ID = {id}";
                sqlCommand.CommandType = CommandType.Text;

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    output = new Contact();
                    output.Id = (int)sqlDataReader["ID"];
                    output.SSN = sqlDataReader["ssn"].ToString();
                    output.FirstName = sqlDataReader["firstname"].ToString();
                    output.LastName = sqlDataReader["lastName"].ToString();
                    output.Email = sqlDataReader["Email"].ToString();
                   
                }
                sqlDataReader.Close();
                sqlConnection.Close();  
            }
           
            return output;
        }

        public List<Contact> ReadAllContacts()
        {
            Contact f = null;
            List<Contact> contacts = new List<Contact>();

            sqlConnection.Open();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = $"select * from Contact";
                sqlCommand.CommandType = CommandType.Text;

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    f = new Contact();
                    f.Id = (int)sqlDataReader["ID"];
                    f.SSN = sqlDataReader["ssn"].ToString();
                    f.FirstName = sqlDataReader["firstname"].ToString();
                    f.LastName = sqlDataReader["lastName"].ToString();
                    f.Email = sqlDataReader["Email"].ToString();
                    contacts.Add(f);
                    
                }
                sqlDataReader.Close();
                sqlConnection.Close();
            }
            sqlConnection.Close();
            return contacts;
        }

        public bool UpdatePost(string procedureToRun, int id, string ssn, string firstName, string lastName, string email)
        {
            bool output;
            sqlConnection.Open();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = procedureToRun;
                sqlCommand.CommandType = CommandType.StoredProcedure;

                AddParameterToSQLContact("@ssn", ssn, 16, sqlCommand);
                AddParameterToSQLContact("@firstName", firstName, 64, sqlCommand);
                AddParameterToSQLContact("@lastName", lastName, 64, sqlCommand);
                AddParameterToSQLContact("@email", email, 64, sqlCommand);
                AddIdParameter("Id", id, sqlCommand);

                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@success";
                parameter.SqlDbType = SqlDbType.Bit;
                parameter.Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add(parameter);
                sqlCommand.ExecuteNonQuery();

                output = (bool)sqlCommand.Parameters["@success"].Value;
                sqlConnection.Close();

            }
            return output;

        }

        public bool DeletePost(string procedureToRun, int id)
        {
            sqlConnection.Open();
            int rowsAffected;

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = procedureToRun;
                sqlCommand.CommandType = CommandType.StoredProcedure;

                AddIdParameter("Id", id, sqlCommand);
                rowsAffected = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

            }
            return rowsAffected > 0 ? true : false;
        }
        private static void AddIdParameter(string paramName, int paramValue, SqlCommand sqlCommand)
        {
            SqlParameter parameter = new SqlParameter();
            parameter.ParameterName = paramName;
            parameter.Value = paramValue;
            parameter.SqlDbType = SqlDbType.Int;
            parameter.Direction = ParameterDirection.Input;
            sqlCommand.Parameters.Add(parameter);
        }
        private static SqlParameter AddParameterToSQLContact(string paramName, string paramValue, int size, SqlCommand sqlCommand)
        {
            SqlParameter parameter = new SqlParameter();
            parameter.ParameterName = paramName;
            parameter.Value = paramValue;
            parameter.SqlDbType = SqlDbType.VarChar;
            parameter.Size = size;
            parameter.Direction = ParameterDirection.Input;
            sqlCommand.Parameters.Add(parameter);
            return parameter;
        }
    }
}
