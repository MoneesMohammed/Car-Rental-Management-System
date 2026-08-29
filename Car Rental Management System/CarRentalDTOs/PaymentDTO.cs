using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalDTOs
{
    public class PaymentDTO
    {
        public enum enPaymentMethod { Cash = 0, CreditCard = 1, DebitCard = 2, BankTransfer = 3 };
        public enum enPaymentStatus { Pending = 0, Paid = 1, Failed = 2, Refunded = 3 };

        public int PaymentID { get; set; }
        public int RentalContractID { get; set; }
        public enPaymentMethod PaymentMethod { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public enPaymentStatus PaymentStatus { get; set; }
        public string? TransactionReference { get; set; }

        public PaymentDTO(int PaymentID, int RentalContractID, enPaymentMethod PaymentMethod, DateTime? PaymentDate, decimal AmountPaid, enPaymentStatus PaymentStatus, string? TransactionReference)
        {
            this.PaymentID = PaymentID;
            this.RentalContractID = RentalContractID;
            this.PaymentMethod = PaymentMethod;
            this.PaymentDate = PaymentDate;
            this.AmountPaid = AmountPaid;
            this.PaymentStatus = PaymentStatus;
            this.TransactionReference = TransactionReference;
        }
    }
}
