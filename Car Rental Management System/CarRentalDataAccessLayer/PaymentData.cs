using CarRentalDTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalDTOs.PaymentDTO;

namespace CarRentalDataAccessLayer
{
    public class clsPaymentData
    {

        public static PaymentDTO? GetPaymentInfoByPaymentID(int PaymentID)
        {

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetPaymentByID";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PaymentID", PaymentID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    DateTime? PaymentDate = reader["PaymentDate"] == DBNull.Value ? null : (DateTime)reader["PaymentDate"];
                    string? TransactionReference = reader["TransactionReference"] == DBNull.Value ? null : (string)reader["TransactionReference"];

                    return new PaymentDTO(PaymentID, (int)reader["RentalContractID"], (enPaymentMethod)(byte)reader["PaymentMethod"], PaymentDate, (decimal)reader["AmountPaid"], (enPaymentStatus)(byte)reader["PaymentStatus"], TransactionReference);
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


        public static int AddNewPayment(PaymentDTO PDTO)
        {
            int ID = -1;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_AddPayment";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;


            command.Parameters.AddWithValue("@RentalContractID", PDTO.RentalContractID);
            command.Parameters.AddWithValue("@PaymentMethod", (byte)PDTO.PaymentMethod);
            command.Parameters.AddWithValue("@AmountPaid", PDTO.AmountPaid);
            command.Parameters.AddWithValue("@PaymentStatus", (byte)PDTO.PaymentStatus);

            if (PDTO.TransactionReference != null)
                command.Parameters.AddWithValue("@TransactionReference", PDTO.TransactionReference);
            else
                command.Parameters.AddWithValue("@TransactionReference", DBNull.Value);

            SqlParameter outputParameter = new SqlParameter("@NewPaymentID", SqlDbType.Int)
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


        public static bool UpdatePayment(PaymentDTO PDTO)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_UpdatePayment";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@PaymentID", PDTO.PaymentID);
            command.Parameters.AddWithValue("@RentalContractID", PDTO.RentalContractID);
            command.Parameters.AddWithValue("@PaymentMethod", (byte)PDTO.PaymentMethod);
            
            command.Parameters.AddWithValue("@AmountPaid", PDTO.AmountPaid);
            command.Parameters.AddWithValue("@PaymentStatus", (byte)PDTO.PaymentStatus);

            if (PDTO.PaymentDate != null)
                command.Parameters.AddWithValue("@PaymentDate", PDTO.PaymentDate);
            else
                command.Parameters.AddWithValue("@PaymentDate", DBNull.Value);


            if (PDTO.TransactionReference != null)
                command.Parameters.AddWithValue("@TransactionReference", PDTO.TransactionReference);
            else
                command.Parameters.AddWithValue("@TransactionReference", DBNull.Value);

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


        public static bool DeletePayment(int PaymentID)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_DeletePayment";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@PaymentID", PaymentID);

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


        public static List<PaymentDTO> GetAllPayments()
        {
            var PaymentList = new List<PaymentDTO>();

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = "SP_GetAllPayments";

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    int PaymentID = (int)reader["PaymentID"];
                    int RentalContractID = (int)reader["RentalContractID"];
                    byte PaymentMethod = (byte)reader["PaymentMethod"];
                    DateTime? PaymentDate = reader["PaymentDate"] == DBNull.Value ? null : (DateTime)reader["PaymentDate"];
                    decimal AmountPaid = (decimal)reader["AmountPaid"];
                    byte PaymentStatus = (byte)reader["PaymentStatus"];
                    string? TransactionReference = reader["TransactionReference"] == DBNull.Value ? null : (string)reader["TransactionReference"];

                    PaymentList.Add(new PaymentDTO(PaymentID, RentalContractID, (enPaymentMethod)PaymentMethod, PaymentDate, AmountPaid, (enPaymentStatus)PaymentStatus, TransactionReference));
                }

                reader.Close();
            }
            catch//(Exception ex)
            { }
            finally
            { connection.Close(); }

            return PaymentList;
        }
    }
}
