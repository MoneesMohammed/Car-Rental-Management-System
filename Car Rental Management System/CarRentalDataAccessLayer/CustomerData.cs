using CarRentalDTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalDTOs.PersonDTO;

namespace CarRentalDataAccessLayer
{
    public class clsCustomerData
    {

        public static CustomerDTO? GetCustomerInfoByCustomerID(int CustomerID)
        {

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetCustomerByID";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CustomerID", CustomerID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    int PersonID = (int)reader["PersonID"];
                    string FirstName = (string)reader["FirstName"];
                    string SecondName = (string)reader["SecondName"];
                    string ThirdName = (string)reader["ThirdName"];
                    string LastName = (string)reader["LastName"];
                    DateTime DateOfBirth = (DateTime)reader["DateOfBirth"];
                    byte Gender = (byte)reader["Gender"];
                    string Address = (string)reader["Address"];
                    string Phone = (string)reader["Phone"];
                    string Email = (string)reader["Email"];

                    string? ImagePath = reader["ImagePath"] == DBNull.Value ? null : (string)reader["ImagePath"];

                    PersonDTO PDTO = new PersonDTO(PersonID, FirstName, SecondName, ThirdName, LastName, DateOfBirth, (enGender)Gender, Address, Phone, Email, ImagePath);



                    return new CustomerDTO(CustomerID, (string)reader["DrivingLicenseNo"], (DateTime)reader["DrivingLicenseExpiryDate"], (bool)reader["IsActive"], (DateTime)reader["CreateDate"] , PDTO);
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


        public static int AddNewCustomer(CustomerDTO CDTO)
        {
            int ID = -1;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_AddCustomer";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@DrivingLicenseNo", CDTO.DrivingLicenseNo);
            command.Parameters.AddWithValue("@DrivingLicenseExpiryDate", CDTO.DrivingLicenseExpiryDate);
            
            command.Parameters.AddWithValue("@FirstName", CDTO.PDTO.FirstName);
            command.Parameters.AddWithValue("@SecondName", CDTO.PDTO.SecondName);
            command.Parameters.AddWithValue("@ThirdName", CDTO.PDTO.ThirdName);
            command.Parameters.AddWithValue("@LastName", CDTO.PDTO.LastName);
            command.Parameters.AddWithValue("@DateOfBirth", CDTO.PDTO.DateOfBirth);
            command.Parameters.AddWithValue("@Gender", (byte)CDTO.PDTO.Gender);
            command.Parameters.AddWithValue("@Address", CDTO.PDTO.Address);
            command.Parameters.AddWithValue("@Phone", CDTO.PDTO.Phone);
            command.Parameters.AddWithValue("@Email", CDTO.PDTO.Email);

            if (CDTO.PDTO.ImagePath != "")
                command.Parameters.AddWithValue("@ImagePath", CDTO.PDTO.ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);


            SqlParameter outputParameter = new SqlParameter("@NewCustomerID", SqlDbType.Int)
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


        public static bool UpdateCustomer(CustomerDTO CDTO)
        {
            bool IsUpdate = false;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_UpdateCustomer";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@CustomerID", CDTO.CustomerID);
            
            command.Parameters.AddWithValue("@DrivingLicenseNo", CDTO.DrivingLicenseNo);
            command.Parameters.AddWithValue("@DrivingLicenseExpiryDate", CDTO.DrivingLicenseExpiryDate);
            command.Parameters.AddWithValue("@IsActive", CDTO.IsActive);
            
            command.Parameters.AddWithValue("@FirstName", CDTO.PDTO.FirstName);
            command.Parameters.AddWithValue("@SecondName", CDTO.PDTO.SecondName);
            command.Parameters.AddWithValue("@ThirdName", CDTO.PDTO.ThirdName);
            command.Parameters.AddWithValue("@LastName", CDTO.PDTO.LastName);
            command.Parameters.AddWithValue("@DateOfBirth", CDTO.PDTO.DateOfBirth);
            command.Parameters.AddWithValue("@Gender", (byte)CDTO.PDTO.Gender);
            command.Parameters.AddWithValue("@Address", CDTO.PDTO.Address);
            command.Parameters.AddWithValue("@Phone", CDTO.PDTO.Phone);
            command.Parameters.AddWithValue("@Email", CDTO.PDTO.Email);

            if (CDTO.PDTO.ImagePath != "")
                command.Parameters.AddWithValue("@ImagePath", CDTO.PDTO.ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", DBNull.Value);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && bool.TryParse(result.ToString(), out bool isUpdate))
                {
                    IsUpdate = isUpdate;
                }
            }
            catch//(Exception ex)
            { return false; }
            finally
            { connection.Close(); }

            return (IsUpdate);
        }


        public static bool DeleteCustomer(int CustomerID)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_DeleteCustomer";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@CustomerID", CustomerID);

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


        public static List<CustomerDTO> GetAllCustomers()
        {
            var CustomerList = new List<CustomerDTO>();

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetAllCustomers";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    int CustomerID = (int)reader["CustomerID"];
                    
                    string DrivingLicenseNo = (string)reader["DrivingLicenseNo"];
                    DateTime DrivingLicenseExpiryDate = (DateTime)reader["DrivingLicenseExpiryDate"];
                    bool IsActive = (bool)reader["IsActive"];
                    DateTime CreateDate = (DateTime)reader["CreateDate"];


                    int PersonID = (int)reader["PersonID"];
                    string FirstName = (string)reader["FirstName"];
                    string SecondName = (string)reader["SecondName"];
                    string ThirdName = (string)reader["ThirdName"];
                    string LastName = (string)reader["LastName"];
                    DateTime DateOfBirth = (DateTime)reader["DateOfBirth"];
                    byte Gender = (byte)reader["Gender"];
                    string Address = (string)reader["Address"];
                    string Phone = (string)reader["Phone"];
                    string Email = (string)reader["Email"];

                    string? ImagePath = reader["ImagePath"] == DBNull.Value ? null : (string)reader["ImagePath"];



                    PersonDTO PDTO = new PersonDTO(PersonID, FirstName, SecondName, ThirdName, LastName, DateOfBirth, (enGender)Gender, Address, Phone, Email, ImagePath);



                    CustomerList.Add(new CustomerDTO(CustomerID, DrivingLicenseNo, DrivingLicenseExpiryDate, IsActive, CreateDate , PDTO));
                }

                reader.Close();
            }
            catch//(Exception ex)
            { }
            finally
            { connection.Close(); }

            return CustomerList;
        }


        public static bool ActivateCustomer(int CustomerID)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_ActivateCustomer";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@CustomerID", CustomerID);

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


        public static List<CustomerDTO> GetAllDeactivateCustomers()
        {
            var CustomerList = new List<CustomerDTO>();

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetAllDeactivateCustomers";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    int CustomerID = (int)reader["CustomerID"];

                    string DrivingLicenseNo = (string)reader["DrivingLicenseNo"];
                    DateTime DrivingLicenseExpiryDate = (DateTime)reader["DrivingLicenseExpiryDate"];
                    bool IsActive = (bool)reader["IsActive"];
                    DateTime CreateDate = (DateTime)reader["CreateDate"];


                    int PersonID = (int)reader["PersonID"];
                    string FirstName = (string)reader["FirstName"];
                    string SecondName = (string)reader["SecondName"];
                    string ThirdName = (string)reader["ThirdName"];
                    string LastName = (string)reader["LastName"];
                    DateTime DateOfBirth = (DateTime)reader["DateOfBirth"];
                    byte Gender = (byte)reader["Gender"];
                    string Address = (string)reader["Address"];
                    string Phone = (string)reader["Phone"];
                    string Email = (string)reader["Email"];

                    string? ImagePath = reader["ImagePath"] == DBNull.Value ? null : (string)reader["ImagePath"];



                    PersonDTO PDTO = new PersonDTO(PersonID, FirstName, SecondName, ThirdName, LastName, DateOfBirth, (enGender)Gender, Address, Phone, Email, ImagePath);



                    CustomerList.Add(new CustomerDTO(CustomerID, DrivingLicenseNo, DrivingLicenseExpiryDate, IsActive, CreateDate, PDTO));
                }

                reader.Close();
            }
            catch//(Exception ex)
            { }
            finally
            { connection.Close(); }

            return CustomerList;
        }


    }

}
