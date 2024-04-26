using HotelWPF.Model;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HotelWPF.DataAccess
{
    public class PaymentDataAccess
    {
        private string connectionString;
        public PaymentDataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public List<Payment> GetPaymentsByReservationId(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<Payment> paymentList = new List<Payment>();

                string query = "SELECT PaymentId, AmountPaid, PaymentDate, PaymentMethod, ReservationId FROM Payment WHERE ReservationId = " + id.ToString();
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    paymentList.Add(new Payment(
                            reader.GetInt32(0),
                            reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                            (float)reader.GetDecimal(1),
                            DateOnly.FromDateTime(reader.GetDateTime(2)),
                            reader.GetString(3)
                        )
                    );
                }
                return paymentList;
            }
        }
        public List<Payment> GetPaymentsByPeriod(DateTime from, DateTime to)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<Payment> paymentList = new List<Payment>();

                string query = "SELECT PaymentId, AmountPaid, PaymentDate, PaymentMethod, ReservationId FROM Payment WHERE PaymentDate >= @DateFrom AND PaymentDate <= @DateTo;";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                command.Parameters.AddWithValue("@DateFrom", from);
                command.Parameters.AddWithValue("@DateTo", to);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    paymentList.Add(new Payment(
                            reader.GetInt32(0),
                            reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                            (float)reader.GetDecimal(1),
                            DateOnly.FromDateTime(reader.GetDateTime(2)),
                            reader.GetString(3)
                        )
                    );
                }
                return paymentList;
            }
        }

        public void ExportPaymentData(DateTime from, DateTime to)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand($"SELECT * FROM Payment WHERE PaymentDate >= @From AND PaymentDate <= @To", connection))
                {
                    command.Parameters.AddWithValue("@From", from);
                    command.Parameters.AddWithValue("@To", to);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        string json = JsonConvert.SerializeObject(table, Newtonsoft.Json.Formatting.Indented);

                        var openFileDialog = new OpenFileDialog();
                        openFileDialog.Title = "Select a file";
                        openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                        openFileDialog.InitialDirectory = @"C:\NULP"; // Set initial directory if needed

                        bool? result = openFileDialog.ShowDialog();

                        if (result == true)
                        {
                            string selectedFileName = openFileDialog.FileName;
                            System.IO.File.WriteAllText(selectedFileName, json);
                        }
                    }
                }
            }
        }

        public bool AddPayment(Payment payment)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Payment (ReservationId, AmountPaid, PaymentDate, PaymentMethod)" +
                    "Values (@ReservationId, @AmountPaid, GETDATE(), @PaymentMethod)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ReservationId", payment.ReservationId);
                    command.Parameters.AddWithValue("@AmountPaid", payment.AmountPaid);
                    command.Parameters.AddWithValue("@PaymentMethod", payment.PaymentMethod);

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
        }
    }
}
