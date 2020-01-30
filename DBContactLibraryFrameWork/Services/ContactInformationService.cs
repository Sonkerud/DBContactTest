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
    public class ContactInformationService : IDisposable
    {
        static SqlConnection sqlConnection = new SqlConnection();

        public ContactInformationService()
        {
            sqlConnection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DBContact;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        public void Dispose()
        {
            sqlConnection.Dispose();
        }

        public ContactInformation ReadContactInformation(int id)
        {

            ContactInformation output = null;

            sqlConnection.Open();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = $"select * from ContactInformation where ContactID = {id}";
                sqlCommand.CommandType = CommandType.Text;

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    output = new ContactInformation();
                    output.Id = (int)sqlDataReader["ID"];
                    output.Info = sqlDataReader["info"].ToString();
                   
                }
                sqlDataReader.Close();
                sqlConnection.Close();
            }

            return output;
        }
    }
}
