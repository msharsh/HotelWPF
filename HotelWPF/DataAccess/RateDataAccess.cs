using HotelWPF.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.DataAccess
{
    public class RateDataAccess
    {
        private string connectionString;

        public RateDataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Rate GetRateById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT RateId, RateType, Multiplier FROM Rate WHERE RateId = " + id.ToString();
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Rate(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        (float)reader.GetDecimal(2)
                    );
                }
                return null;
            }
        }
    }
}
