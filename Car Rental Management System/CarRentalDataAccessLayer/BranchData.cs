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
    public class clsBranchData
    {

        public static BranchDTO? GetBranchInfoByBranchID(int BranchID)
        {

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetBranchByID";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@BranchID", BranchID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new BranchDTO(BranchID, (string)reader["BranchName"], (string)reader["Address"], (string)reader["City"], (string)reader["Phone"], (bool)reader["IsActive"]);
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


        public static int AddNewBranch(BranchDTO BDTO)
        {
            int ID = -1;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_AddBranch";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;


            command.Parameters.AddWithValue("@BranchName", BDTO.BranchName);
            command.Parameters.AddWithValue("@Address", BDTO.Address);
            command.Parameters.AddWithValue("@City", BDTO.City);
            command.Parameters.AddWithValue("@Phone", BDTO.Phone);
            

            SqlParameter outputParameter = new SqlParameter("@NewBranchID", SqlDbType.Int)
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


        public static bool UpdateBranch(BranchDTO BDTO)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_UpdateBranch";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@BranchID", BDTO.BranchID);
            command.Parameters.AddWithValue("@BranchName", BDTO.BranchName);
            command.Parameters.AddWithValue("@Address", BDTO.Address);
            command.Parameters.AddWithValue("@City", BDTO.City);
            command.Parameters.AddWithValue("@Phone", BDTO.Phone);
            command.Parameters.AddWithValue("@IsActive", BDTO.IsActive);

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


        public static bool DeleteBranch(int BranchID)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_DeleteBranch";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@BranchID", BranchID);

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


        public static List<BranchDTO> GetAllBranches()
        {
            var BrancheList = new List<BranchDTO>();

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetAllBranches";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    int BranchID = (int)reader["BranchID"];
                    string BranchName = (string)reader["BranchName"];
                    string Address = (string)reader["Address"];
                    string City = (string)reader["City"];
                    string Phone = (string)reader["Phone"];
                    bool IsActive = (bool)reader["IsActive"];

                    BrancheList.Add(new BranchDTO(BranchID, BranchName, Address, City, Phone, IsActive));
                }

                reader.Close();
            }
            catch//(Exception ex)
            { }
            finally
            { connection.Close(); }

            return BrancheList;
        }



        public static bool ActivateBranch(int BranchID)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_ActivateBranch";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@BranchID", BranchID);

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

        public static List<BranchDTO> GetAllDeactivateBranches()
        {
            var BrancheList = new List<BranchDTO>();

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetAllDeactivateBranches";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    int BranchID = (int)reader["BranchID"];
                    string BranchName = (string)reader["BranchName"];
                    string Address = (string)reader["Address"];
                    string City = (string)reader["City"];
                    string Phone = (string)reader["Phone"];
                    bool IsActive = (bool)reader["IsActive"];

                    BrancheList.Add(new BranchDTO(BranchID, BranchName, Address, City, Phone, IsActive));
                }

                reader.Close();
            }
            catch//(Exception ex)
            { }
            finally
            { connection.Close(); }

            return BrancheList;
        }



    }
}
