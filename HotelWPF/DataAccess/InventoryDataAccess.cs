using HotelWPF.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.DataAccess
{
    class InventoryDataAccess
    {
        private string connectionString;

        public InventoryDataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Inventory GetInventoryById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT InventoryId, ItemName, Description FROM Inventory WHERE InventoryId = " + id.ToString();
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Inventory(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2)
                    );
                }
                return null;
            }
        }

        public Dictionary<Inventory, int> GetInventoryByRoomType(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Dictionary<Inventory, int> roomInventory = new Dictionary<Inventory, int>();

                string query = "SELECT InventoryId, Quantity FROM RoomInventory WHERE RoomInventory.RoomTypeId = " + id.ToString();
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Inventory inventory = GetInventoryById(reader.GetInt32(0));

                    roomInventory.Add(inventory, reader.GetInt32(1));
                }
                return roomInventory;
            }
        }
    }
}
