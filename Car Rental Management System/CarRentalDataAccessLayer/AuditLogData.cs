using CarRentalDataAccessLayer.Global_Classes;
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
    public class clsAuditLogData
    {
        public static AuditLogDTO? GetAuditLogInfoByAuditLogID(int AuditLogID)
        {

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetAuditLogByID";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@AuditLogID", AuditLogID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int? EntityID = reader["EntityID"] == DBNull.Value ? null : (int?)reader["EntityID"];
                    string? OldValue = reader["OldValue"] == DBNull.Value ? null : (string?)reader["OldValue"];
                    string? NewValue = reader["NewValue"] == DBNull.Value ? null : (string?)reader["NewValue"];

                    return new AuditLogDTO(AuditLogID, (int)reader["UserID"], (string)reader["Action"], (string)reader["EntityName"], EntityID, OldValue, NewValue, (DateTime)reader["CreatedAt"]);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                clsLogger.LogError(ex, "An error occurred");
                throw;
            }
            finally
            {
                connection.Close();
            }

            return null;
        }

        public static int AddNewAuditLog(AuditLogDTO ALDTO)
        {
            int ID = -1;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_AddAuditLog";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@UserID", ALDTO.UserID);
            command.Parameters.AddWithValue("@Action", ALDTO.Action);
            command.Parameters.AddWithValue("@EntityName", ALDTO.EntityName);
            command.Parameters.AddWithValue("@CreatedAt", ALDTO.CreatedAt);

            if (ALDTO.EntityID != null)
                command.Parameters.AddWithValue("@EntityID", ALDTO.EntityID);
            else
                command.Parameters.AddWithValue("@EntityID", DBNull.Value);

            if (ALDTO.OldValue != null)
                command.Parameters.AddWithValue("@OldValue", ALDTO.OldValue);
            else
                command.Parameters.AddWithValue("@OldValue", DBNull.Value);

            if (ALDTO.NewValue != null)
                command.Parameters.AddWithValue("@NewValue", ALDTO.NewValue);
            else
                command.Parameters.AddWithValue("@NewValue", DBNull.Value);

            SqlParameter outputParameter = new SqlParameter("@NewAuditLogID", SqlDbType.Int)
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
            catch (Exception ex)
            {
                clsLogger.LogError(ex, "An error occurred");
                throw;
            }
            finally
            { connection.Close(); }

            return ID;
        }

        public static List<AuditLogDTO> GetAllAuditLogs()
        {
            var AuditLogList = new List<AuditLogDTO>();

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetAllAuditLogs";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    int AuditLogID = (int)reader["AuditLogID"];
                    int UserID = (int)reader["UserID"];
                    string Action = (string)reader["Action"];
                    string EntityName = (string)reader["EntityName"];
                    DateTime CreatedAt = (DateTime)reader["CreatedAt"];

                    int? EntityID    = reader["EntityID"] == DBNull.Value ? null : (int?)reader["EntityID"];
                    string? OldValue = reader["OldValue"] == DBNull.Value ? null : (string?)reader["OldValue"];
                    string? NewValue = reader["NewValue"] == DBNull.Value ? null : (string?)reader["NewValue"];

                    AuditLogList.Add(new AuditLogDTO(AuditLogID, UserID, Action, EntityName, EntityID, OldValue, NewValue, CreatedAt));
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                clsLogger.LogError(ex, "An error occurred");
                throw;
            }
            finally
            { connection.Close(); }

            return AuditLogList;
        }
    }

}
