using DBContactLibraryFrameWork.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContactLibraryFrameWork.Services
{
    public class C2AService : IDisposable
    {
        static SqlConnection sqlConnection = new SqlConnection();

        public C2AService()
        {
            sqlConnection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DBContact;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        }

        public Address ReadContactsAddress(int id)
        {

            Address output = null;

            sqlConnection.Open();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = $"select * from Contact join ContactToAddress on Contact.Id = ContactToAddress.ContactID join [Address] on ContactToAddress.AddressID = [Address].ID where Contact.Id = {id}";
                sqlCommand.CommandType = CommandType.Text;

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    output = new Address();
                    output.Id = (int)sqlDataReader["ID"];
                    output.City = sqlDataReader["city"].ToString();
                    output.Street = sqlDataReader["street"].ToString();
                    output.Zip = (int)sqlDataReader["zip"];
                }
                sqlDataReader.Close();
                sqlConnection.Close();
            }

            return output;
        }

        public void Dispose()
        {
            sqlConnection.Dispose();
        }


    }
}
