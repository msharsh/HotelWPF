using HotelWPF.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.DataAccess
{
    public class RoomTypeDataAccess
    {
        private string connectionString;

        public RoomTypeDataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<RoomType> GetRoomTypes()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<RoomType> roomTypes = new List<RoomType>();

                string query = "SELECT RoomTypeId, TypeName, Capacity, Description, StandartRate, RateId FROM RoomType";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    roomTypes.Add(new RoomType(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetInt32(2),
                        reader.GetString(3),
                        (float)reader.GetDecimal(4),
                        new RateDataAccess(connectionString).GetRateById(reader.GetInt32(5)),
                        new InventoryDataAccess(connectionString).GetInventoryByRoomType(reader.GetInt32(0))
                    ));
                }
                return roomTypes;
            }
        }

        public RoomType GetRoomTypeById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT RoomTypeId, TypeName, Capacity, Description, StandartRate, RateId FROM RoomType WHERE RoomTypeId = " + id.ToString();
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new RoomType(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetInt32(2),
                        reader.GetString(3),
                        (float) reader.GetDecimal(4),
                        new RateDataAccess(connectionString).GetRateById(reader.GetInt32(5)),
                        new InventoryDataAccess(connectionString).GetInventoryByRoomType(reader.GetInt32(0))
                    );
                }
                return null;
            }
        }

        public RoomType GetRoomTypeByName(string name)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT RoomTypeId, TypeName, Capacity, Description, StandartRate, RateId FROM RoomType WHERE TypeName = " + name;
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new RoomType(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetInt32(2),
                        reader.GetString(3),
                        (float)reader.GetDecimal(4),
                        new RateDataAccess(connectionString).GetRateById(reader.GetInt32(5)),
                        new InventoryDataAccess(connectionString).GetInventoryByRoomType(reader.GetInt32(0))
                    );
                }
                return null;
            }
        }

        public int FindRoomTypeCountByName(string name)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT RoomTypeCount FROM dbo.FindRoomTypeCountByName(@Name);";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", name);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return reader.GetInt32(0);
                }
                return 0;
            }
        }
    }
}
