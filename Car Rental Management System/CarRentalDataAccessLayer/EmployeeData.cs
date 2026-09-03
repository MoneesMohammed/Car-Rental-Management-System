using CarRentalDTOs;
using Microsoft.Data.SqlClient;
using System.Data;
using static CarRentalDTOs.PersonDTO;

namespace CarRentalDataAccessLayer
{
    public class clsEmployeeData
    {

        public static EmployeeDTO? GetEmployeeInfoByEmployeeID(int EmployeeID)
        {

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetEmployeeByID";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@EmployeeID", EmployeeID);

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

                    return new EmployeeDTO(EmployeeID,(int)reader["JobTitleID"], (int)reader["WorkingBranchID"], (DateTime)reader["HireDate"], (bool)reader["IsActive"], PDTO);
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


        public static int AddNewEmployee(EmployeeDTO EDTO , ref int PersonID)
        {
            int EmployeeID = -1 ;
           
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_AddEmployee";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@JobTitleID", EDTO.JobTitleID);
            command.Parameters.AddWithValue("@WorkingBranchID", EDTO.WorkingBranchID);
            command.Parameters.AddWithValue("@HireDate", EDTO.HireDate);
            
            command.Parameters.AddWithValue("@FirstName", EDTO.PDTO.FirstName);
            command.Parameters.AddWithValue("@SecondName", EDTO.PDTO.SecondName);
            command.Parameters.AddWithValue("@ThirdName", EDTO.PDTO.ThirdName);
            command.Parameters.AddWithValue("@LastName", EDTO.PDTO.LastName);
            command.Parameters.AddWithValue("@DateOfBirth", EDTO.PDTO.DateOfBirth);
            command.Parameters.AddWithValue("@Gender", (byte)EDTO.PDTO.Gender);
            command.Parameters.AddWithValue("@Address", EDTO.PDTO.Address);
            command.Parameters.AddWithValue("@Phone", EDTO.PDTO.Phone);
            command.Parameters.AddWithValue("@Email", EDTO.PDTO.Email);

            if (EDTO.PDTO.ImagePath != null)
                command.Parameters.AddWithValue("@ImagePath", EDTO.PDTO.ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

            SqlParameter EmployeeIDParam = new SqlParameter("@NewEmployeeID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(EmployeeIDParam);

            SqlParameter PersonIDParam = new SqlParameter("@NewPersonID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(PersonIDParam);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();

                if (EmployeeIDParam.Value != DBNull.Value)
                {
                    EmployeeID = (int)EmployeeIDParam.Value;
                }

                if (PersonIDParam.Value != DBNull.Value)
                {
                    PersonID = (int)PersonIDParam.Value;
                }

            }
            catch//(Exception ex)
            { EmployeeID = -1; }
            finally
            { connection.Close(); }

            return EmployeeID;
        }


        public static bool UpdateEmployee(EmployeeDTO EDTO)
        {
            bool IsUpdate = false;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_UpdateEmployee";
      
            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@EmployeeID", EDTO.EmployeeID);

            command.Parameters.AddWithValue("@JobTitleID", EDTO.JobTitleID);
            command.Parameters.AddWithValue("@WorkingBranchID", EDTO.WorkingBranchID);
            command.Parameters.AddWithValue("@HireDate", EDTO.HireDate);
            command.Parameters.AddWithValue("@IsActive", EDTO.IsActive);

            command.Parameters.AddWithValue("@FirstName", EDTO.PDTO.FirstName);
            command.Parameters.AddWithValue("@SecondName", EDTO.PDTO.SecondName);
            command.Parameters.AddWithValue("@ThirdName", EDTO.PDTO.ThirdName);
            command.Parameters.AddWithValue("@LastName", EDTO.PDTO.LastName);
            command.Parameters.AddWithValue("@DateOfBirth", EDTO.PDTO.DateOfBirth);
            command.Parameters.AddWithValue("@Gender", (byte)EDTO.PDTO.Gender);
            command.Parameters.AddWithValue("@Address", EDTO.PDTO.Address);
            command.Parameters.AddWithValue("@Phone", EDTO.PDTO.Phone);
            command.Parameters.AddWithValue("@Email", EDTO.PDTO.Email);

            if (EDTO.PDTO.ImagePath != null)
                command.Parameters.AddWithValue("@ImagePath", EDTO.PDTO.ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);


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


        public static bool DeleteEmployee(int EmployeeID)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_DeleteEmployee";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@EmployeeID", EmployeeID);

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


        public static List<EmployeeDTO> GetAllEmployees()
        {
            var EmployeeList = new List<EmployeeDTO>();

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetAllEmployees";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    int EmployeeID = (int)reader["EmployeeID"];
                    
                    int JobTitleID = (int)reader["JobTitleID"];
                    int WorkingBranchID = (int)reader["WorkingBranchID"];
                    DateTime HireDate = (DateTime)reader["HireDate"];
                    bool IsActive = (bool)reader["IsActive"];


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

                    EmployeeList.Add(new EmployeeDTO(EmployeeID, JobTitleID, WorkingBranchID, HireDate, IsActive, PDTO));
                }

                reader.Close();
            }
            catch//(Exception ex)
            { }
            finally
            { connection.Close(); }

            return EmployeeList;
        }

        public static bool ActivateEmployee(int EmployeeID)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_ActivateEmployee";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@EmployeeID", EmployeeID);

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

        public static List<EmployeeDTO> GetAllDeactivateEmployees()
        {
            var EmployeeList = new List<EmployeeDTO>();

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetAllDeactivateEmployees";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    int EmployeeID = (int)reader["EmployeeID"];

                    int JobTitleID = (int)reader["JobTitleID"];
                    int WorkingBranchID = (int)reader["WorkingBranchID"];
                    DateTime HireDate = (DateTime)reader["HireDate"];
                    bool IsActive = (bool)reader["IsActive"];


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

                    EmployeeList.Add(new EmployeeDTO(EmployeeID, JobTitleID, WorkingBranchID, HireDate, IsActive, PDTO));
                }

                reader.Close();
            }
            catch//(Exception ex)
            { }
            finally
            { connection.Close(); }

            return EmployeeList;
        }

    }

}
