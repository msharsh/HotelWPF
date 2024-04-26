using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.DataAccess
{
    public static class LoginDataAccess
    {
        public static bool TryLogin(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException ex)
                {
                    return false;
                }
            }
        }

        public static UserRole GetRole(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new(@"SELECT r.name AS [Role]
                     FROM sys.database_role_members m
                     INNER JOIN sys.database_principals u ON m.member_principal_id = u.principal_id
                     INNER JOIN sys.database_principals r ON m.role_principal_id = r.principal_id
                     WHERE u.name = '" + GetUsername(connectionString) + "';", connection);

                switch((string)command.ExecuteScalar())
                {
                    case "manager":
                        return UserRole.Manager;
                    case "accountant":
                        return UserRole.Accountant;
                    case "receptionist":
                        return UserRole.Receptionist;
                    default:
                        return UserRole.Default;
                }
            }
        }
        public static string GetUsername(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new("SELECT SUSER_SNAME() AS CurrentUser;", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return reader.GetString(0);
                }
                return "";
            }
        }
    }

    public enum UserRole
    {
        Manager,
        Accountant,
        Receptionist,
        Default
    }
}
