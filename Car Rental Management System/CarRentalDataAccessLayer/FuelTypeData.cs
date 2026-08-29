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
    public class clsFuelTypeData
    {
        public static FuelTypeDTO? GetFuelTypeInfoByFuelTypeID(int FuelTypeID)
        {

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetFuelTypeByID";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@FuelTypeID", FuelTypeID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new FuelTypeDTO(FuelTypeID, (string)reader["FuelType"]);
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

        public static List<FuelTypeDTO> GetAllFuelTypes()
        {
            var FuelTypeList = new List<FuelTypeDTO>();

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetAllFuelTypes";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    int FuelTypeID = (int)reader["FuelTypeID"];
                    string FuelType = (string)reader["FuelType"];

                    FuelTypeList.Add(new FuelTypeDTO(FuelTypeID, FuelType));
                }

                reader.Close();
            }
            catch//(Exception ex)
            { }
            finally
            { connection.Close(); }

            return FuelTypeList;
        }
    }

}
