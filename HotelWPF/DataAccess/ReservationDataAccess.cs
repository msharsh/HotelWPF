using HotelWPF.Model;
using HotelWPF.QueryOptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HotelWPF.DataAccess
{
    public class ReservationDataAccess
    {
        private string connectionString;
        public ReservationDataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Reservation> GetReservations(int page, int perPage)
        {
            List<Reservation> reservations = new List<Reservation>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ReservationId, RoomId, GuestName, PhoneNumber, CheckInDate, CheckOutDate, PaymentStatus FROM Reservation " +
                    "ORDER BY ReservationId " +
                    "OFFSET (@Page - 1) * @PerPage ROWS " +
                    "FETCH NEXT @PerPage ROWS ONLY;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Page", page);
                command.Parameters.AddWithValue("@PerPage", perPage);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Room? room = new RoomDataAccess(connectionString).GetRooms().Where(e => e.Id == reader.GetInt32(1)).FirstOrDefault();

                    Reservation reservation = new Reservation
                    (
                        reader.GetInt32(0),
                        room,
                        reader.GetString(2),
                        reader.GetString(3),
                        DateOnly.FromDateTime(reader.GetDateTime(4)),
                        DateOnly.FromDateTime(reader.GetDateTime(5)),
                        reader.GetString(6),
                        new PaymentDataAccess(connectionString).GetPaymentsByReservationId(reader.GetInt32(0)),
                        new ServiceDataAccess(connectionString).GetServicesByReservation(reader.GetInt32(0)),
                        new DiscountDataAccess(connectionString).GetDiscountsByReservation(reader.GetInt32(0))
                    );
                    reservations.Add(reservation);
                }
            }

            return reservations;
        }

        public List<Reservation> GetReservationsWithOptions(int page, int perPage, ReservationGetOptions options)
        {
            List<Reservation> reservations = new List<Reservation>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ReservationId, RoomId, GuestName, PhoneNumber, CheckInDate, CheckOutDate, PaymentStatus FROM Reservation " +
                    "WHERE GuestName LIKE @SearchName";
                if (options.ShowOnlyUnpaid)
                {
                    query += " AND PaymentStatus = 'Not Paid'";
                }
                if (options.FilterByDate)
                {
                    query += " AND CheckInDate <= @FilterDate AND CheckOutDate >= @FilterDate";
                }
                if (options.EnableSort)
                {
                    query += " ORDER BY " + options.SortColumnName;
                }
                else
                {
                    query += " ORDER BY ReservationId";
                }
                query +=    " OFFSET (@Page - 1) * @PerPage ROWS " +
                            "FETCH NEXT @PerPage ROWS ONLY;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SearchName", options.NameFilter + "%");
                command.Parameters.AddWithValue("@Page", page);
                command.Parameters.AddWithValue("@PerPage", perPage);
                if (options.FilterByDate) command.Parameters.AddWithValue("@FilterDate", options.FilterDate);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Room? room = new RoomDataAccess(connectionString).GetRooms().Where(e => e.Id == reader.GetInt32(1)).FirstOrDefault();

                    Reservation reservation = new Reservation
                    (
                        reader.GetInt32(0),
                        room,
                        reader.GetString(2),
                        reader.GetString(3),
                        DateOnly.FromDateTime(reader.GetDateTime(4)),
                        DateOnly.FromDateTime(reader.GetDateTime(5)),
                        reader.GetString(6),
                        new PaymentDataAccess(connectionString).GetPaymentsByReservationId(reader.GetInt32(0)),
                        new ServiceDataAccess(connectionString).GetServicesByReservation(reader.GetInt32(0)),
                        new DiscountDataAccess(connectionString).GetDiscountsByReservation(reader.GetInt32(0))
                    );
                    reservations.Add(reservation);
                }
            }

            return reservations;
        }

        public List<Reservation> GetFutureArrivals()
        {
            List<Reservation> reservations = new List<Reservation>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ReservationId, RoomId, GuestName, PhoneNumber, CheckInDate, CheckOutDate, PaymentStatus FROM Reservation " +
                    "WHERE ABS(DATEDIFF(day, GETDATE(), CheckInDate)) <= 7 ORDER BY ReservationId;";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Room? room = new RoomDataAccess(connectionString).GetRooms().Where(e => e.Id == reader.GetInt32(1)).FirstOrDefault();

                    Reservation reservation = new Reservation
                    (
                        reader.GetInt32(0),
                        room,
                        reader.GetString(2),
                        reader.GetString(3),
                        DateOnly.FromDateTime(reader.GetDateTime(4)),
                        DateOnly.FromDateTime(reader.GetDateTime(5)),
                        reader.GetString(6),
                        new PaymentDataAccess(connectionString).GetPaymentsByReservationId(reader.GetInt32(0)),
                        new ServiceDataAccess(connectionString).GetServicesByReservation(reader.GetInt32(0)),
                        new DiscountDataAccess(connectionString).GetDiscountsByReservation(reader.GetInt32(0))
                    );
                    reservations.Add(reservation);
                }
            }

            return reservations;
        }

        public List<Reservation> GetFutureDepartures()
        {
            List<Reservation> reservations = new List<Reservation>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ReservationId, RoomId, GuestName, PhoneNumber, CheckInDate, CheckOutDate, PaymentStatus FROM Reservation " +
                    "WHERE ABS(DATEDIFF(day, GETDATE(), CheckOutDate)) <= 7 ORDER BY ReservationId;";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Room? room = new RoomDataAccess(connectionString).GetRooms().Where(e => e.Id == reader.GetInt32(1)).FirstOrDefault();

                    Reservation reservation = new Reservation
                    (
                        reader.GetInt32(0),
                        room,
                        reader.GetString(2),
                        reader.GetString(3),
                        DateOnly.FromDateTime(reader.GetDateTime(4)),
                        DateOnly.FromDateTime(reader.GetDateTime(5)),
                        reader.GetString(6),
                        new PaymentDataAccess(connectionString).GetPaymentsByReservationId(reader.GetInt32(0)),
                        new ServiceDataAccess(connectionString).GetServicesByReservation(reader.GetInt32(0)),
                        new DiscountDataAccess(connectionString).GetDiscountsByReservation(reader.GetInt32(0))
                    );
                    reservations.Add(reservation);
                }
            }

            return reservations;
        }

        public float GetReservationCost(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT TotalCost FROM dbo.CalculateReservationCost(@Id);";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return (float) reader.GetDouble(0);
                }
                return 0;
            }
        }

        public List<Reservation> GetUnpaidReservations()
        {
            List<Reservation> reservations = new List<Reservation>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ReservationId, RoomId, GuestName, PhoneNumber, CheckInDate, CheckOutDate, PaymentStatus FROM Reservation " +
                    "WHERE PaymentStatus = 'Not Paid' ORDER BY ReservationId;";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Room? room = new RoomDataAccess(connectionString).GetRooms().Where(e => e.Id == reader.GetInt32(1)).FirstOrDefault();

                    Reservation reservation = new Reservation
                    (
                        reader.GetInt32(0),
                        room,
                        reader.GetString(2),
                        reader.GetString(3),
                        DateOnly.FromDateTime(reader.GetDateTime(4)),
                        DateOnly.FromDateTime(reader.GetDateTime(5)),
                        reader.GetString(6),
                        new PaymentDataAccess(connectionString).GetPaymentsByReservationId(reader.GetInt32(0)),
                        new ServiceDataAccess(connectionString).GetServicesByReservation(reader.GetInt32(0)),
                        new DiscountDataAccess(connectionString).GetDiscountsByReservation(reader.GetInt32(0))
                    );
                    reservations.Add(reservation);
                }
            }

            return reservations;
        }

        public bool AddReservation(Reservation reservation)
        {
            int lastId = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                string query = "INSERT INTO Reservation (RoomId, GuestName, PhoneNumber, CheckInDate, CheckOutDate, PaymentStatus)" +
                    "OUTPUT INSERTED.ReservationId " +
                    "Values (@RoomId, @GuestName, @PhoneNumber, @CheckInDate, @CheckOutDate, @PaymentStatus)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomId", reservation.Room.Id);
                    command.Parameters.AddWithValue("@GuestName", reservation.GuestName);
                    command.Parameters.AddWithValue("@PhoneNumber", reservation.PhoneNumber);
                    command.Parameters.AddWithValue("@CheckInDate", reservation.CheckInDate.ToDateTime(TimeOnly.MinValue));
                    command.Parameters.AddWithValue("@CheckOutDate", reservation.CheckOutDate.ToDateTime(TimeOnly.MinValue));
                    command.Parameters.AddWithValue("@PaymentStatus", reservation.PaymentStatus);

                    lastId = (int)command.ExecuteScalar();

                }
            }
            new DiscountDataAccess(connectionString).AddDiscountsToReservation(reservation.AppliedDiscounts, lastId);
            new ServiceDataAccess(connectionString).AddServicesToReservation(reservation.ReservedServices, lastId);

            return true;
        }

        public bool UpdateReservation(Reservation reservation)
        {
            new DiscountDataAccess(connectionString).DeleteDiscountsByReservation(reservation.Id);
            new ServiceDataAccess(connectionString).DeleteServicesByReservation(reservation.Id);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Reservation " +
                    "SET RoomId = @RoomId, GuestName = @GuestName, PhoneNumber = @PhoneNumber, CheckInDate = @CheckInDate, CheckOutDate = @CheckOutDate, PaymentStatus = @PaymentStatus " +
                    "WHERE ReservationId = @ReservationId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ReservationId", reservation.Id);
                    command.Parameters.AddWithValue("@RoomId", reservation.Room.Id);
                    command.Parameters.AddWithValue("@GuestName", reservation.GuestName);
                    command.Parameters.AddWithValue("@PhoneNumber", reservation.PhoneNumber);
                    command.Parameters.AddWithValue("@CheckInDate", reservation.CheckInDate.ToDateTime(TimeOnly.MinValue));
                    command.Parameters.AddWithValue("@CheckOutDate", reservation.CheckOutDate.ToDateTime(TimeOnly.MinValue));
                    command.Parameters.AddWithValue("@PaymentStatus", reservation.PaymentStatus);

                    int rowsAffected = command.ExecuteNonQuery();
                }
            }

            new DiscountDataAccess(connectionString).AddDiscountsToReservation(reservation.AppliedDiscounts, reservation.Id);
            new ServiceDataAccess(connectionString).AddServicesToReservation(reservation.ReservedServices, reservation.Id);

            return true;
        }

        public bool DeleteReservation(int id)
        {
            new DiscountDataAccess(connectionString).DeleteDiscountsByReservation(id);
            new ServiceDataAccess(connectionString).DeleteServicesByReservation(id);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Reservation WHERE ReservationId = @ReservationId";
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
