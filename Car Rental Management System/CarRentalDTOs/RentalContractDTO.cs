using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalDTOs
{
    public class RentalContractDTO
    {
        public enum enContractStatus { Active = 0, Completed = 1, Cancelled = 2 };

        public int RentalContractID { get; set; }
        public string ContractNumber { get; set; }
        public int ReservationID { get; set; }
        public DateTime ActualPickupDateTime { get; set; }
        public DateTime? ActualReturnDateTime { get; set; }
        public int OdometerAtPickup { get; set; }
        public int? OdometerAtReturn { get; set; }
        public byte FuelLevelUponReceipt { get; set; } // 0 between 100
        public byte? FuelLevelUponReturn { get; set; } // 0 between 100
        public decimal BasicPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal AdditionalFeesAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public enContractStatus ContractStatus { get; set; }

        public RentalContractDTO(int RentalContractID, string ContractNumber, int ReservationID, DateTime ActualPickupDateTime, DateTime? ActualReturnDateTime, int OdometerAtPickup, int? OdometerAtReturn, byte FuelLevelUponReceipt, byte? FuelLevelUponReturn, decimal BasicPrice, decimal DiscountAmount, decimal AdditionalFeesAmount, decimal TotalAmount, enContractStatus ContractStatus)
        {
            this.RentalContractID = RentalContractID;
            this.ContractNumber = ContractNumber;
            this.ReservationID = ReservationID;
            this.ActualPickupDateTime = ActualPickupDateTime;
            this.ActualReturnDateTime = ActualReturnDateTime;
            this.OdometerAtPickup = OdometerAtPickup;
            this.OdometerAtReturn = OdometerAtReturn;
            this.FuelLevelUponReceipt = FuelLevelUponReceipt;
            this.FuelLevelUponReturn = FuelLevelUponReturn;
            this.BasicPrice = BasicPrice;
            this.DiscountAmount = DiscountAmount;
            this.AdditionalFeesAmount = AdditionalFeesAmount;
            this.TotalAmount = TotalAmount;
            this.ContractStatus = ContractStatus;
        }
    }

}
