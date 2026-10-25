using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalDTOs
{
    public class PaymentSummaryDTO
    {
        public int RentalContractID { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingBalance { get; set; }
        public string PaymentStatus { get; set; }


        public PaymentSummaryDTO(int RentalContractID , decimal TotalAmount , decimal PaidAmount , decimal RemainingBalance , string PaymentStatus) 
        {
            this.RentalContractID = RentalContractID;
            this.TotalAmount = TotalAmount;
            this.PaidAmount = PaidAmount;
            this.RemainingBalance = RemainingBalance;
            this.PaymentStatus = PaymentStatus;
        }

    }


}
