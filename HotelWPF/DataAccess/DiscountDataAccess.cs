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
    public class DiscountDataAccess
    {
        private string connectionString;
        public DiscountDataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Discount> GetDiscounts()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<Discount> discounts = new List<Discount>();

                string query = "SELECT DiscountId, DiscountType, DiscountValue, Description FROM Discount";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    discounts.Add(new Discount(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        (float)reader.GetDecimal(2),
                        reader.GetString(3)
                    ));
                }
                return discounts;
            }
        }

        public Discount GetDiscountById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT DiscountId, DiscountType, DiscountValue, Description FROM Discount WHERE DiscountId = " + id.ToString();
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Discount(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        (float)reader.GetDecimal(2),
                        reader.GetString(3)
                    );
                }
                return null;
            }
        }

        public List<Discount> GetDiscountsByReservation(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<Discount> appliedDiscounts = new List<Discount>();

                string query = "SELECT DiscountId FROM ReservationDiscount WHERE ReservationDiscount.ReservationId = " + id.ToString();
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Discount discount = GetDiscountById(reader.GetInt32(0));

                    appliedDiscounts.Add(discount);
                }
                return appliedDiscounts;
            }
        }

        public bool AddDiscountsToReservation(List<Discount> discounts, int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO ReservationDiscount (ReservationId, DiscountId) Values (@ReservationId, @DiscountId)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    int rowsAffected = 0;

                    command.Parameters.AddWithValue("@ReservationId", id);
                    command.Parameters.Add("@DiscountId", SqlDbType.Int);

                    foreach (var discount in discounts)
                    {
                        command.Parameters["@DiscountId"].Value = discount.Id;

                        rowsAffected += command.ExecuteNonQuery();
                    }
                    return rowsAffected != 0;
                }
            }
        }

        public bool DeleteDiscountsByReservation(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM ReservationDiscount WHERE ReservationId = @ReservationId";
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
