using CarRentalDTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalDTOs.RentalContractDTO;

namespace CarRentalDataAccessLayer
{
    public class clsRentalContractData
    {

        public static RentalContractDTO? GetRentalContractInfoByRentalContractID(int RentalContractID)
        {

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetRentalContractByID";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@RentalContractID", RentalContractID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    DateTime? ActualReturnDateTime = reader["ActualReturnDateTime"] == DBNull.Value ? null : (DateTime)reader["ActualReturnDateTime"];
                    int?      OdometerAtReturn     = reader["OdometerAtReturn"]     == DBNull.Value ? null : (int)reader["OdometerAtReturn"];
                    byte?     FuelLevelUponReturn  = reader["FuelLevelUponReturn"]  == DBNull.Value ? null : (byte)reader["FuelLevelUponReturn"];

                    return new RentalContractDTO(RentalContractID, (string)reader["ContractNumber"], (int)reader["ReservationID"], (DateTime)reader["ActualPickupDateTime"], ActualReturnDateTime, (int)reader["OdometerAtPickup"], OdometerAtReturn, (byte)reader["FuelLevelUponReceipt"], FuelLevelUponReturn, (decimal)reader["BasicPrice"], (decimal)reader["DiscountAmount"], (decimal)reader["AdditionalFeesAmount"], (decimal)reader["TotalAmount"], (enContractStatus)(byte)reader["ContractStatus"]);
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


        public static int AddNewRentalContract(RentalContractDTO RCDTO)
        {
            int ID = -1;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_AddRentalContract";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;


            command.Parameters.AddWithValue("@ContractNumber", RCDTO.ContractNumber);
            command.Parameters.AddWithValue("@ReservationID", RCDTO.ReservationID);
            command.Parameters.AddWithValue("@ActualPickupDateTime", RCDTO.ActualPickupDateTime);
            
            command.Parameters.AddWithValue("@OdometerAtPickup", RCDTO.OdometerAtPickup);
           
            command.Parameters.AddWithValue("@FuelLevelUponReceipt", RCDTO.FuelLevelUponReceipt);
            
            command.Parameters.AddWithValue("@BasicPrice", RCDTO.BasicPrice);
            command.Parameters.AddWithValue("@DiscountAmount", RCDTO.DiscountAmount);
            command.Parameters.AddWithValue("@AdditionalFeesAmount", RCDTO.AdditionalFeesAmount);
            command.Parameters.AddWithValue("@TotalAmount", RCDTO.TotalAmount);



            SqlParameter outputParameter = new SqlParameter("@NewRentalContractID", SqlDbType.Int)
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


        public static bool UpdateRentalContract(RentalContractDTO RCDTO)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_UpdateRentalContract";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@RentalContractID", RCDTO.RentalContractID);
            command.Parameters.AddWithValue("@ContractNumber", RCDTO.ContractNumber);
            command.Parameters.AddWithValue("@ReservationID", RCDTO.ReservationID);
            command.Parameters.AddWithValue("@ActualPickupDateTime", RCDTO.ActualPickupDateTime);
            command.Parameters.AddWithValue("@OdometerAtPickup", RCDTO.OdometerAtPickup);
            command.Parameters.AddWithValue("@FuelLevelUponReceipt", RCDTO.FuelLevelUponReceipt);
            command.Parameters.AddWithValue("@BasicPrice", RCDTO.BasicPrice);
            command.Parameters.AddWithValue("@DiscountAmount", RCDTO.DiscountAmount);
            command.Parameters.AddWithValue("@AdditionalFeesAmount", RCDTO.AdditionalFeesAmount);
            command.Parameters.AddWithValue("@TotalAmount", RCDTO.TotalAmount);
            command.Parameters.AddWithValue("@ContractStatus", (byte)RCDTO.ContractStatus);

            if (RCDTO.ActualReturnDateTime != null)
                command.Parameters.AddWithValue("@ActualReturnDateTime", RCDTO.ActualReturnDateTime);
            else
                command.Parameters.AddWithValue("@ActualReturnDateTime", DBNull.Value);

            if (RCDTO.OdometerAtReturn != null)
                command.Parameters.AddWithValue("@OdometerAtReturn", RCDTO.OdometerAtReturn);
            else
                command.Parameters.AddWithValue("@OdometerAtReturn", DBNull.Value);

            if (RCDTO.FuelLevelUponReturn != null)
                command.Parameters.AddWithValue("@FuelLevelUponReturn", RCDTO.FuelLevelUponReturn);
            else
                command.Parameters.AddWithValue("@FuelLevelUponReturn", DBNull.Value);


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


        public static bool DeleteRentalContract(int RentalContractID)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_DeleteRentalContract";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@RentalContractID", RentalContractID);

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


        public static List<RentalContractDTO> GetAllRentalContracts()
        {
            var RentalContractList = new List<RentalContractDTO>();

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetAllRentalContracts";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    int RentalContractID = (int)reader["RentalContractID"];
                    string ContractNumber = (string)reader["ContractNumber"];
                    int ReservationID = (int)reader["ReservationID"];
                    DateTime ActualPickupDateTime = (DateTime)reader["ActualPickupDateTime"];
                    int OdometerAtPickup = (int)reader["OdometerAtPickup"];
                    byte FuelLevelUponReceipt = (byte)reader["FuelLevelUponReceipt"];
                    decimal BasicPrice = (decimal)reader["BasicPrice"];
                    decimal DiscountAmount = (decimal)reader["DiscountAmount"];
                    decimal AdditionalFeesAmount = (decimal)reader["AdditionalFeesAmount"];
                    decimal TotalAmount = (decimal)reader["TotalAmount"];
                    byte ContractStatus = (byte)reader["ContractStatus"];


                    DateTime? ActualReturnDateTime = reader["ActualReturnDateTime"] == DBNull.Value ? null : (DateTime)reader["ActualReturnDateTime"];
                    int? OdometerAtReturn = reader["OdometerAtReturn"] == DBNull.Value ? null : (int)reader["OdometerAtReturn"];
                    byte? FuelLevelUponReturn = reader["FuelLevelUponReturn"] == DBNull.Value ? null : (byte)reader["FuelLevelUponReturn"];


                    RentalContractList.Add(new RentalContractDTO(RentalContractID, ContractNumber, ReservationID, ActualPickupDateTime, ActualReturnDateTime, OdometerAtPickup, OdometerAtReturn, FuelLevelUponReceipt, FuelLevelUponReturn, BasicPrice, DiscountAmount, AdditionalFeesAmount, TotalAmount, (enContractStatus)ContractStatus));
                }

                reader.Close();
            }
            catch//(Exception ex)
            { }
            finally
            { connection.Close(); }

            return RentalContractList;
        }
    }
}
