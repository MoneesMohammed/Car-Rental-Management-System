using Microsoft.Data.SqlClient;
using System;
using System.Data;
using CarRentalDTOs;

namespace CarRentalDataAccessLayer
{
    public class clsPersonData
    {

        public static PersonDTO? GetPersonInfoByPersonID(int PersonID)
        {

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetPersonByID";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new PersonDTO(PersonID, (string)reader["FirstName"], (string)reader["SecondName"], (string)reader["ThirdName"], (string)reader["LastName"], (DateTime)reader["DateOfBirth"], (PersonDTO.enGender)reader["Gender"], (string)reader["Address"], (string)reader["Phone"], (string)reader["Email"], (string)reader["ImagePath"]);
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


        public static int AddNewPerson(PersonDTO PDTO)
        {
            int ID = -1;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_AddPerson";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;


            command.Parameters.AddWithValue("@FirstName", PDTO.FirstName);
            command.Parameters.AddWithValue("@SecondName", PDTO.SecondName);
            command.Parameters.AddWithValue("@ThirdName", PDTO.ThirdName);
            command.Parameters.AddWithValue("@LastName", PDTO.LastName);
            command.Parameters.AddWithValue("@DateOfBirth", PDTO.DateOfBirth);
            command.Parameters.AddWithValue("@Gender", PDTO.Gender);
            command.Parameters.AddWithValue("@Address", PDTO.Address);
            command.Parameters.AddWithValue("@Phone", PDTO.Phone);
            command.Parameters.AddWithValue("@Email", PDTO.Email);
            command.Parameters.AddWithValue("@ImagePath", PDTO.ImagePath);

            SqlParameter outputParameter = new SqlParameter("@NewPersonID", SqlDbType.Int)
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


        public static bool UpdatePerson(PersonDTO PDTO)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_UpdatePerson";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@PersonID", PDTO.PersonID);
            command.Parameters.AddWithValue("@FirstName", PDTO.FirstName);
            command.Parameters.AddWithValue("@SecondName", PDTO.SecondName);
            command.Parameters.AddWithValue("@ThirdName", PDTO.ThirdName);
            command.Parameters.AddWithValue("@LastName", PDTO.LastName);
            command.Parameters.AddWithValue("@DateOfBirth", PDTO.DateOfBirth);
            command.Parameters.AddWithValue("@Gender", PDTO.Gender);
            command.Parameters.AddWithValue("@Address", PDTO.Address);
            command.Parameters.AddWithValue("@Phone", PDTO.Phone);
            command.Parameters.AddWithValue("@Email", PDTO.Email);
            command.Parameters.AddWithValue("@ImagePath", PDTO.ImagePath);

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


        public static bool DeletePerson(int PersonID)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_DeletePerson";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@PersonID", PersonID);

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


        public static List<PersonDTO> GetAllPeople()
        {
            var PersonList = new List<PersonDTO>();

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetAllStudents";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    int PersonID = (int)reader["PersonID"];
                    string FirstName = (string)reader["FirstName"];
                    string SecondName = (string)reader["SecondName"];
                    string ThirdName = (string)reader["ThirdName"];
                    string LastName = (string)reader["LastName"];
                    DateTime DateOfBirth = (DateTime)reader["DateOfBirth"];
                    PersonDTO.enGender Gender = (PersonDTO.enGender)reader["Gender"];
                    string Address = (string)reader["Address"];
                    string Phone = (string)reader["Phone"];
                    string Email = (string)reader["Email"];
                    string ImagePath = (string)reader["ImagePath"];

                    PersonList.Add(new PersonDTO(PersonID, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Address, Phone, Email, ImagePath));
                }

                reader.Close();
            }
            catch//(Exception ex)
            { }
            finally
            { connection.Close(); }

            return PersonList;
        }


        public static bool IsEmailUnique(string Email)
        {
            bool IsUnique = false;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_CheckEmailUnique";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Email", Email);


            SqlParameter isUniqueParam = new SqlParameter("@IsUnique", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(isUniqueParam);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();

                if (isUniqueParam.Value != DBNull.Value)
                {
                    IsUnique = (bool)isUniqueParam.Value;
                }

            }
            catch//(Exception ex)
            { return false; }
            finally
            { connection.Close(); }

            return IsUnique;
        }
















    }

}
