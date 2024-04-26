using HotelWPF.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.DataAccess
{
    public class ExpenseDataAccess
    {
        private string connectionString;
        public ExpenseDataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public List<Expense> GetExpensesForPeriod(DateTime from, DateTime to)
        {
            List<Expense> expenses = new List<Expense>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Amount, ExpenseDate FROM Expense WHERE ExpenseDate >= @From AND ExpenseDate <= @To";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@From", from);
                command.Parameters.AddWithValue("@To", to);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    expenses.Add(new Expense(
                        (float)reader.GetDecimal(0),
                        reader.GetDateTime(1)
                    ));
                }
            }

            return expenses;
        }
    }
}
