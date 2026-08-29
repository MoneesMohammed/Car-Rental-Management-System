using CarRentalDTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalDTOs.MaintenanceDTO;

namespace CarRentalDataAccessLayer
{
    public class clsMaintenanceData
    {
        public static MaintenanceDTO? GetMaintenanceInfoByMaintenanceID(int MaintenanceID)
        {

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetMaintenanceByID";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@MaintenanceID", MaintenanceID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    DateTime? EndDate = reader["EndDate"] == DBNull.Value ? null : (DateTime)reader["EndDate"];
                    return new MaintenanceDTO(MaintenanceID, (int)reader["CarID"], (DateTime)reader["StartDate"], EndDate, (enMaintenanceType)(byte)reader["MaintenanceType"], (string)reader["ProblemDescription"], (decimal)reader["Cost"], (enMaintenanceStatus)(byte)reader["MaintenanceStatus"]);
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


        public static int AddNewMaintenance(MaintenanceDTO MDTO)
        {
            int ID = -1;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_AddMaintenance";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;


            command.Parameters.AddWithValue("@CarID", MDTO.CarID);
            command.Parameters.AddWithValue("@StartDate", MDTO.StartDate);
            
            command.Parameters.AddWithValue("@MaintenanceType", (byte)MDTO.MaintenanceType);
            command.Parameters.AddWithValue("@ProblemDescription", MDTO.ProblemDescription);
            command.Parameters.AddWithValue("@Cost", MDTO.Cost);
            command.Parameters.AddWithValue("@MaintenanceStatus", (byte)MDTO.MaintenanceStatus);

            SqlParameter outputParameter = new SqlParameter("@NewMaintenanceID", SqlDbType.Int)
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


        public static bool UpdateMaintenance(MaintenanceDTO MDTO)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_UpdateMaintenance";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@MaintenanceID", MDTO.MaintenanceID);
            command.Parameters.AddWithValue("@CarID", MDTO.CarID);
            command.Parameters.AddWithValue("@StartDate", MDTO.StartDate);
            
            command.Parameters.AddWithValue("@MaintenanceType", (byte)MDTO.MaintenanceType);
            command.Parameters.AddWithValue("@ProblemDescription", MDTO.ProblemDescription);
            command.Parameters.AddWithValue("@Cost", MDTO.Cost);
            command.Parameters.AddWithValue("@MaintenanceStatus", (byte)MDTO.MaintenanceStatus);

            if(MDTO.EndDate != null)
                command.Parameters.AddWithValue("@EndDate", MDTO.EndDate);
            else
                command.Parameters.AddWithValue("@EndDate", DBNull.Value);

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


        public static bool DeleteMaintenance(int MaintenanceID)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_DeleteMaintenance";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@MaintenanceID", MaintenanceID);

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


        public static List<MaintenanceDTO> GetAllMaintenance()
        {
            var MaintenanceList = new List<MaintenanceDTO>();

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetAllMaintenances";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    int MaintenanceID = (int)reader["MaintenanceID"];
                    int CarID = (int)reader["CarID"];
                    DateTime StartDate = (DateTime)reader["StartDate"];
                    DateTime? EndDate = reader["EndDate"] == DBNull.Value ? null : (DateTime)reader["EndDate"];
                    byte MaintenanceType = (byte)reader["MaintenanceType"];
                    string ProblemDescription = (string)reader["ProblemDescription"];
                    decimal Cost = (decimal)reader["Cost"];
                    byte MaintenanceStatus = (byte)reader["MaintenanceStatus"];

                    MaintenanceList.Add(new MaintenanceDTO(MaintenanceID, CarID, StartDate, EndDate, (enMaintenanceType)MaintenanceType, ProblemDescription, Cost, (enMaintenanceStatus)MaintenanceStatus));
                }

                reader.Close();
            }
            catch//(Exception ex)
            { }
            finally
            { connection.Close(); }

            return MaintenanceList;
        }
    }
}
