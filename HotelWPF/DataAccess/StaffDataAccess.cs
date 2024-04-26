using HotelWPF.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.DataAccess
{
    public class StaffDataAccess
    {
        private string connectionString;
        public StaffDataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public float GetSalarySum()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT SUM(Salary) AS TotalSalary FROM Staff;";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return (float)reader.GetDecimal(0);
                }
                return 0;
            }
        }
    }
}
