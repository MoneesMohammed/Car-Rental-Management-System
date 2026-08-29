using CarRentalDTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalDataAccessLayer
{
    public class clsUserData
    {

        public static UserDTO? GetUserInfoByUserID(int UserID)
        {

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetUserByID";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    DateTime? LastLogin = reader["LastLogin"] == DBNull.Value ? null : (DateTime)reader["LastLogin"];

                    return new UserDTO(UserID, (int)reader["EmployeeID"], (int)reader["RoleID"], (string)reader["RoleName"], (string)reader["UserName"], (string)reader["PasswordHash"], (bool)reader["IsActive"], (DateTime)reader["CreateDate"], LastLogin);
                }

                reader.Close();
            }
            catch//(Exception ex)
            {
                return null;
            }
            finally
            {
                connection.Close();
            }

            return null;
        }


        public static int AddNewUser(UserDTO UDTO)
        {
            int ID = -1;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_AddNewUser";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;


            command.Parameters.AddWithValue("@EmployeeID", UDTO.EmployeeID);
            command.Parameters.AddWithValue("@RoleID", UDTO.RoleID);
            command.Parameters.AddWithValue("@UserName", UDTO.UserName);
            command.Parameters.AddWithValue("@PasswordHash", UDTO.PasswordHash);
            
            

            SqlParameter outputParameter = new SqlParameter("@NewUserID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(outputParameter);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();

                if (outputParameter.Value != DBNull.Value)
                {
                    ID = (int)outputParameter.Value;
                }

            }
            catch//(Exception ex)
            { ID = -1; }
            finally
            { connection.Close(); }

            return ID;
        }


        public static bool UpdateUser(UserDTO UDTO)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_UpdateUser";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@UserID", UDTO.UserID);
            command.Parameters.AddWithValue("@EmployeeID", UDTO.EmployeeID);
            command.Parameters.AddWithValue("@RoleID", UDTO.RoleID);
            command.Parameters.AddWithValue("@UserName", UDTO.UserName);
            command.Parameters.AddWithValue("@PasswordHash", UDTO.PasswordHash);
            command.Parameters.AddWithValue("@IsActive", UDTO.IsActive);
            
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int affectedRows))
                {
                    RowAffected = affectedRows;
                }
            }
            catch//(Exception ex)
            { return false; }
            finally
            { connection.Close(); }

            return (RowAffected > 0);
        }


        public static bool DeleteUser(int UserID)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_DeleteUser";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int affectedRows))
                {
                    RowAffected = affectedRows;
                }
            }
            catch//(Exception ex)
            { return false; }
            finally
            { connection.Close(); }

            return (RowAffected > 0);
        }


        public static List<UserDTO> GetAllUsers()
        {
            var UserList = new List<UserDTO>();

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetAllUsers";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    int UserID = (int)reader["UserID"];
                    int EmployeeID = (int)reader["EmployeeID"];
                    int RoleID = (int)reader["RoleID"];
                    string RoleName = (string)reader["RoleName"];
                    string UserName = (string)reader["UserName"];
                    string PasswordHash = (string)reader["PasswordHash"];
                    bool IsActive = (bool)reader["IsActive"];
                    DateTime CreateDate = (DateTime)reader["CreateDate"];
                    DateTime? LastLogin = reader["LastLogin"] == DBNull.Value ? null : (DateTime)reader["LastLogin"];

                    UserList.Add(new UserDTO(UserID, EmployeeID, RoleID, RoleName, UserName, PasswordHash, IsActive, CreateDate, LastLogin));
                }

                reader.Close();
            }
            catch//(Exception ex)
            { }
            finally
            { connection.Close(); }

            return UserList;
        }

        public static bool ActivateUser(int UserID)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_ActivateUser";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int affectedRows))
                {
                    RowAffected = affectedRows;
                }
            }
            catch //(Exception ex)
            {

                return false;
            }
            finally
            { connection.Close(); }

            return (RowAffected > 0);
        }


        public static bool UpdateLastLoginDate(int UserID)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_UpdateLastLoginDate";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int affectedRows))
                {
                    RowAffected = affectedRows;
                }
            }
            catch //(Exception ex)
            {

                return false;
            }
            finally
            { connection.Close(); }

            return (RowAffected > 0);
        }


        public static List<UserDTO> GetAllDeactivateUsers()
        {
            var UserList = new List<UserDTO>();

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetAllDeactivateUsers";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    int UserID = (int)reader["UserID"];
                    int EmployeeID = (int)reader["EmployeeID"];
                    int RoleID = (int)reader["RoleID"];
                    string RoleName = (string)reader["RoleName"];
                    string UserName = (string)reader["UserName"];
                    string PasswordHash = (string)reader["PasswordHash"];
                    bool IsActive = (bool)reader["IsActive"];
                    DateTime CreateDate = (DateTime)reader["CreateDate"];
                    DateTime? LastLogin = reader["LastLogin"] == DBNull.Value ? null : (DateTime)reader["LastLogin"];

                    UserList.Add(new UserDTO(UserID, EmployeeID, RoleID, RoleName, UserName, PasswordHash, IsActive, CreateDate, LastLogin));
                }

                reader.Close();
            }
            catch//(Exception ex)
            { }
            finally
            { connection.Close(); }

            return UserList;
        }


        public static UserDTO? GetUserInfoByUserName(string UserName)
        {

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetUserByUserName";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    DateTime? LastLogin = reader["LastLogin"] == DBNull.Value ? null : (DateTime)reader["LastLogin"];

                    return new UserDTO( (int)reader["UserID"], (int)reader["EmployeeID"], (int)reader["RoleID"], (string)reader["RoleName"], UserName, (string)reader["PasswordHash"], (bool)reader["IsActive"], (DateTime)reader["CreateDate"], LastLogin);
                }

                reader.Close();
            }
            catch//(Exception ex)
            {
                return null;
            }
            finally
            {
                connection.Close();
            }

            return null;
        }

    }

}
