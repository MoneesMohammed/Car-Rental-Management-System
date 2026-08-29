using CarRentalDTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalDTOs.CarDTO;

namespace CarRentalDataAccessLayer
{
    public class clsCarData
    {

        public static CarDTO? GetCarInfoByCarID(int CarID)
        {

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetCarByID";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CarID", CarID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int? CurrentBranchID = reader["CurrentBranchID"] == DBNull.Value ? null : (int)reader["CurrentBranchID"];

                    byte TransmissionType = (byte)reader["TransmissionType"];
                    byte CarStatus = (byte)reader["CarStatus"];

                    return new CarDTO(CarID, (string)reader["Make"], (string)reader["Model"], (int)reader["Year"], (string)reader["PlateNumber"], (string)reader["VIN"], (string)reader["Color"], (int)reader["NumberOfSeats"], (enTransmissionType)TransmissionType, (int)reader["FuelTypeID"], (int)reader["CarCategoryID"], (enCarStatus)CarStatus, (decimal)reader["DailyRentalPrice"], (int)reader["CurrentMileage"], CurrentBranchID );
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


        public static int AddNewCar(CarDTO CDTO)
        {
            int ID = -1;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_AddCar";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;


            command.Parameters.AddWithValue("@Make", CDTO.Make);
            command.Parameters.AddWithValue("@Model", CDTO.Model);
            command.Parameters.AddWithValue("@Year", CDTO.Year);
            command.Parameters.AddWithValue("@PlateNumber", CDTO.PlateNumber);
            command.Parameters.AddWithValue("@VIN", CDTO.VIN);
            command.Parameters.AddWithValue("@Color", CDTO.Color);
            command.Parameters.AddWithValue("@NumberOfSeats", CDTO.NumberOfSeats);
            command.Parameters.AddWithValue("@TransmissionType", (byte)CDTO.TransmissionType);
            command.Parameters.AddWithValue("@FuelTypeID", CDTO.FuelTypeID);
            command.Parameters.AddWithValue("@CarCategoryID", CDTO.CarCategoryID);
            command.Parameters.AddWithValue("@CarStatus", (byte)CDTO.CarStatus);
            command.Parameters.AddWithValue("@DailyRentalPrice", CDTO.DailyRentalPrice);
            command.Parameters.AddWithValue("@CurrentMileage", CDTO.CurrentMileage);

            if(CDTO.CurrentBranchID != null)
                command.Parameters.AddWithValue("@CurrentBranchID", CDTO.CurrentBranchID);
            else
                command.Parameters.AddWithValue("@CurrentBranchID", DBNull.Value);

            SqlParameter outputParameter = new SqlParameter("@NewCarID", SqlDbType.Int)
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


        public static bool UpdateCar(CarDTO CDTO)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_UpdateCar";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@CarID", CDTO.CarID);
            command.Parameters.AddWithValue("@Make", CDTO.Make);
            command.Parameters.AddWithValue("@Model", CDTO.Model);
            command.Parameters.AddWithValue("@Year", CDTO.Year);
            command.Parameters.AddWithValue("@PlateNumber", CDTO.PlateNumber);
            command.Parameters.AddWithValue("@VIN", CDTO.VIN);
            command.Parameters.AddWithValue("@Color", CDTO.Color);
            command.Parameters.AddWithValue("@NumberOfSeats", CDTO.NumberOfSeats);
            command.Parameters.AddWithValue("@TransmissionType", (byte)CDTO.TransmissionType);
            command.Parameters.AddWithValue("@FuelTypeID", CDTO.FuelTypeID);
            command.Parameters.AddWithValue("@CarCategoryID", CDTO.CarCategoryID);
            command.Parameters.AddWithValue("@CarStatus", (byte)CDTO.CarStatus);
            command.Parameters.AddWithValue("@DailyRentalPrice", CDTO.DailyRentalPrice);
            command.Parameters.AddWithValue("@CurrentMileage", CDTO.CurrentMileage);

            if (CDTO.CurrentBranchID != null)
                command.Parameters.AddWithValue("@CurrentBranchID", CDTO.CurrentBranchID);
            else
                command.Parameters.AddWithValue("@CurrentBranchID", DBNull.Value);

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


        public static bool DeleteCar(int CarID)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_DeleteCar";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@CarID", CarID);

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


        public static List<CarDTO> GetAllCars()
        {
            var CarList = new List<CarDTO>();

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetAllCars";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    int CarID = (int)reader["CarID"];
                    string Make = (string)reader["Make"];
                    string Model = (string)reader["Model"];
                    int Year = (int)reader["Year"];
                    string PlateNumber = (string)reader["PlateNumber"];
                    string VIN = (string)reader["VIN"];
                    string Color = (string)reader["Color"];
                    int NumberOfSeats = (int)reader["NumberOfSeats"];
                    byte TransmissionType = (byte)reader["TransmissionType"];
                    int FuelTypeID = (int)reader["FuelTypeID"];
                    int CarCategoryID = (int)reader["CarCategoryID"];
                    byte CarStatus = (byte)reader["CarStatus"];
                    decimal DailyRentalPrice = (decimal)reader["DailyRentalPrice"];
                    int CurrentMileage = (int)reader["CurrentMileage"];

                    int? CurrentBranchID = reader["CurrentBranchID"] == DBNull.Value ? null : (int)reader["CurrentBranchID"];

                    CarList.Add(new CarDTO(CarID, Make, Model, Year, PlateNumber, VIN, Color, NumberOfSeats, (enTransmissionType)TransmissionType, FuelTypeID, CarCategoryID, (enCarStatus)CarStatus, DailyRentalPrice, CurrentMileage, CurrentBranchID));
                }

                reader.Close();
            }
            catch//(Exception ex)
            { }
            finally
            { connection.Close(); }

            return CarList;
        }


        public static bool CheckCarAvailability(int CarID, ref string Message)
        {
            bool IsAvailable = false;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_CheckCarAvailability";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@CarID", CarID);


            SqlParameter isAvailableParam = new SqlParameter("@IsAvailable", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(isAvailableParam);

            SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar)
            {
                Size = 250,
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(messageParam);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();

                if (isAvailableParam.Value != DBNull.Value)
                {
                    IsAvailable = (bool)isAvailableParam.Value;
                }

                if (messageParam.Value != DBNull.Value)
                {
                    Message = (string)messageParam.Value;
                }


            }
            catch//(Exception ex)
            { return false; }
            finally
            { connection.Close(); }

            return IsAvailable;
        }




    }

}
