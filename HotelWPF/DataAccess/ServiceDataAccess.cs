using HotelWPF.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.DataAccess
{
    public class ServiceDataAccess
    {
        private string connectionString;
        public ServiceDataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Service> GetServices()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<Service> services = new List<Service>();

                string query = "SELECT ServiceId, ServiceName, Description, Cost FROM Service";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    services.Add(new Service(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        (float)reader.GetDecimal(3)
                    ));
                }
                return services;
            }
        }

        public Service GetServiceById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ServiceId, ServiceName, Description, Cost FROM Service WHERE ServiceId = " + id.ToString();
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Service(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        (float)reader.GetDecimal(3)
                    );
                }
                return null;
            }
        }

        public List<Service> GetServicesByReservation(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<Service> reservedServices = new List<Service>();

                string query = "SELECT ServiceId FROM ServiceReservation WHERE ServiceReservation.ReservationId = " + id.ToString();
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Service service = GetServiceById(reader.GetInt32(0));

                    reservedServices.Add(service);
                }
                return reservedServices;
            }
        }

        public int FindServiceCountByName(string name)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ServiceCount FROM dbo.FindServiceCountByName(@Name);";
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

        public bool AddServicesToReservation(List<Service> services, int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO ServiceReservation (ReservationId, ServiceId) Values (@ReservationId, @ServiceId)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    int rowsAffected = 0;

                    command.Parameters.AddWithValue("@ReservationId", id);
                    command.Parameters.Add("@ServiceId", SqlDbType.Int);

                    foreach (var service in services)
                    {
                        command.Parameters["@ServiceId"].Value = service.Id;

                        rowsAffected += command.ExecuteNonQuery();
                    }
                    return rowsAffected != 0;
                }
            }
        }

        public bool DeleteServicesByReservation(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM ServiceReservation WHERE ReservationId = @ReservationId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ReservationId", id);

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
        }
    }
}
