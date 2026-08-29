using CarRentalDTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalDTOs.ReservationDTO;

namespace CarRentalDataAccessLayer
{
    public class clsReservationData
    {

        public static ReservationDTO? GetReservationInfoByReservationID(int ReservationID)
        {

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetReservationByID";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ReservationID", ReservationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    byte BookingStatus = (byte)reader["BookingStatus"];

                    return new ReservationDTO(ReservationID, (int)reader["CustomerID"], (int)reader["CarID"], (int)reader["PickupBranchID"], (int)reader["ReturnsBranchID"], (DateTime)reader["PickupDateTime"], (DateTime)reader["ExpectedReturnDateTime"], (decimal)reader["AgreedPrice"], (enBookingStatus)BookingStatus);
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


        public static int AddNewReservation(ReservationDTO RDTO)
        {
            int ID = -1;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_AddReservation";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;


            command.Parameters.AddWithValue("@CustomerID", RDTO.CustomerID);
            command.Parameters.AddWithValue("@CarID", RDTO.CarID);
            command.Parameters.AddWithValue("@PickupBranchID", RDTO.PickupBranchID);
            command.Parameters.AddWithValue("@ReturnsBranchID", RDTO.ReturnsBranchID);
            command.Parameters.AddWithValue("@PickupDateTime", RDTO.PickupDateTime);
            command.Parameters.AddWithValue("@ExpectedReturnDateTime", RDTO.ExpectedReturnDateTime);
            command.Parameters.AddWithValue("@AgreedPrice", RDTO.AgreedPrice);
            command.Parameters.AddWithValue("@BookingStatus", (byte)RDTO.BookingStatus);

            SqlParameter outputParameter = new SqlParameter("@NewReservationID", SqlDbType.Int)
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


        public static bool UpdateReservation(ReservationDTO RDTO)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_UpdateReservation";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ReservationID", RDTO.ReservationID);
            command.Parameters.AddWithValue("@CustomerID", RDTO.CustomerID);
            command.Parameters.AddWithValue("@CarID", RDTO.CarID);
            command.Parameters.AddWithValue("@PickupBranchID", RDTO.PickupBranchID);
            command.Parameters.AddWithValue("@ReturnsBranchID", RDTO.ReturnsBranchID);
            command.Parameters.AddWithValue("@PickupDateTime", RDTO.PickupDateTime);
            command.Parameters.AddWithValue("@ExpectedReturnDateTime", RDTO.ExpectedReturnDateTime);
            command.Parameters.AddWithValue("@AgreedPrice", RDTO.AgreedPrice);
            command.Parameters.AddWithValue("@BookingStatus", (byte)RDTO.BookingStatus);

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


        public static bool DeleteReservation(int ReservationID)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_DeleteReservation";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ReservationID", ReservationID);

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


        public static List<ReservationDTO> GetAllReservations()
        {
            var ReservationList = new List<ReservationDTO>();

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetAllReservations";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    int ReservationID = (int)reader["ReservationID"];
                    int CustomerID = (int)reader["CustomerID"];
                    int CarID = (int)reader["CarID"];
                    int PickupBranchID = (int)reader["PickupBranchID"];
                    int ReturnsBranchID = (int)reader["ReturnsBranchID"];
                    DateTime PickupDateTime = (DateTime)reader["PickupDateTime"];
                    DateTime ExpectedReturnDateTime = (DateTime)reader["ExpectedReturnDateTime"];
                    decimal AgreedPrice = (decimal)reader["AgreedPrice"];
                    byte BookingStatus = (byte)reader["BookingStatus"];

                    ReservationList.Add(new ReservationDTO(ReservationID, CustomerID, CarID, PickupBranchID, ReturnsBranchID, PickupDateTime, ExpectedReturnDateTime, AgreedPrice, (enBookingStatus)BookingStatus));
                }

                reader.Close();
            }
            catch//(Exception ex)
            { }
            finally
            { connection.Close(); }

            return ReservationList;
        }


    }
}
