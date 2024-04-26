using HotelWPF.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.DataAccess
{
    internal class RoomDataAccess
    {
        private string connectionString;

        public RoomDataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Room> GetRooms()
        {
            List<Room> rooms = new List<Room>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT RoomId, RoomNumber, Floor, RoomTypeId, Status FROM Room";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Room room = new Room
                    (
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetInt32(2),
                        new RoomTypeDataAccess(connectionString).GetRoomTypeById(reader.GetInt32(3)),
                        reader.GetString(4)
                    );
                    rooms.Add(room);
                }
            }

            return rooms;
        }

        public List<Room> GetAvailableRooms(int excludeReservation, DateTime from, DateTime to)
        {
            string sqlCommandText = "SELECT RoomId FROM dbo.GetAvailableRooms(@Reservation, @DateFrom, @DateTo) ORDER BY RoomID";
            List<Room> allRooms = GetRooms();
            List<Room> rooms = new List<Room>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlCommandText, connection))
                {
                    command.Parameters.AddWithValue("@Reservation", excludeReservation);
                    command.Parameters.AddWithValue("@DateFrom", from);
                    command.Parameters.AddWithValue("@DateTo", to);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Room? room = allRooms.FirstOrDefault(e => e.Id == reader.GetInt32(0));
                        if (room != null)
                        {
                            rooms.Add(room);
                        }
                    }
                }
            }
            return rooms;
        }

        public bool AddRoom(Room room)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Room (RoomNumber, Floor, RoomTypeId, Status) Values (@RoomNumber, @Floor, @RoomTypeId, @Status)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);
                    command.Parameters.AddWithValue("@Floor", room.Floor);
                    command.Parameters.AddWithValue("@RoomTypeId", room.RoomType.Id);
                    command.Parameters.AddWithValue("@Status", room.Status);

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
        }

        public bool UpdateRoom(Room room)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE Room SET RoomNumber = @RoomNumber, Floor = @Floor, RoomTypeId = @RoomTypeId, Status = @Status WHERE RoomId = @RoomId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomId", room.Id);
                    command.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);
                    command.Parameters.AddWithValue("@Floor", room.Floor);
                    command.Parameters.AddWithValue("@RoomTypeId", room.RoomType.Id);
                    command.Parameters.AddWithValue("@Status", room.Status);

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
        }

        public bool DeleteRoom(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Room WHERE RoomId = @RoomId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomId", id);

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
        }
    }
}
