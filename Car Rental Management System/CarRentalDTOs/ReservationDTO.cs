using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalDTOs
{
    public class ReservationDTO
    {
        public enum enBookingStatus {Pending = 0 , Confirmed = 1 ,  Cancelled = 2 ,Completed = 3 , NoShow = 4};

        public int ReservationID { get; set; }
        public int CustomerID { get; set; }
        public int CarID { get; set; }
        public int PickupBranchID { get; set; }
        public int ReturnsBranchID { get; set; }
        public DateTime PickupDateTime { get; set; }
        public DateTime ExpectedReturnDateTime { get; set; }
        public decimal AgreedPrice { get; set; }
        public enBookingStatus BookingStatus { get; set; }

        public ReservationDTO(int ReservationID, int CustomerID, int CarID, int PickupBranchID, int ReturnsBranchID, DateTime PickupDateTime, DateTime ExpectedReturnDateTime, decimal AgreedPrice, enBookingStatus BookingStatus)
        {
            this.ReservationID = ReservationID;
            this.CustomerID = CustomerID;
            this.CarID = CarID;
            this.PickupBranchID = PickupBranchID;
            this.ReturnsBranchID = ReturnsBranchID;
            this.PickupDateTime = PickupDateTime;
            this.ExpectedReturnDateTime = ExpectedReturnDateTime;
            this.AgreedPrice = AgreedPrice;
            this.BookingStatus = BookingStatus;
        }
    }
}
