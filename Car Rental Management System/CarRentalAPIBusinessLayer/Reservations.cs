using CarRentalDataAccessLayer;
using CarRentalDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalDTOs.ReservationDTO;

namespace CarRentalAPIBusinessLayer
{
    public class clsReservation
    {


        public enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;




        public int ReservationID { get; set; }
        public int CustomerID { get; set; }
        public int CarID { get; set; }
        public int PickupBranchID { get; set; }
        public int ReturnsBranchID { get; set; }
        public DateTime PickupDateTime { get; set; }
        public DateTime ExpectedReturnDateTime { get; set; }
        public decimal AgreedPrice { get; set; }
        public enBookingStatus BookingStatus { get; set; }


        public ReservationDTO RDTO
        {
            get { return new ReservationDTO(this.ReservationID, this.CustomerID, this.CarID, this.PickupBranchID, this.ReturnsBranchID, this.PickupDateTime, this.ExpectedReturnDateTime, this.AgreedPrice, this.BookingStatus); }
        }


        public clsReservation(ReservationDTO RDTO, enMode cMode = enMode.AddNew)
        {
            this.ReservationID = RDTO.ReservationID;
            this.CustomerID = RDTO.CustomerID;
            this.CarID = RDTO.CarID;
            this.PickupBranchID = RDTO.PickupBranchID;
            this.ReturnsBranchID = RDTO.ReturnsBranchID;
            this.PickupDateTime = RDTO.PickupDateTime;
            this.ExpectedReturnDateTime = RDTO.ExpectedReturnDateTime;
            this.AgreedPrice = RDTO.AgreedPrice;
            this.BookingStatus = RDTO.BookingStatus;

            this.Mode = cMode;
        }


        public static clsReservation? Find(int ReservationID)
        {
            ReservationDTO? RDTO = clsReservationData.GetReservationInfoByReservationID(ReservationID);

            if (RDTO != null)
            {
                return new clsReservation(RDTO, enMode.Update);
            }
            else
                return null;
        }


        private bool _AddNewReservation()
        {
            this.ReservationID = clsReservationData.AddNewReservation(RDTO);
            return (ReservationID != -1);
        }


        private bool _UpdateReservation()
        {
            return clsReservationData.UpdateReservation(RDTO);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:

                    if (_AddNewReservation())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:

                    return (_UpdateReservation());

            }

            return false;
        }


        public static bool DeleteReservation(int ReservationID)
        {
            return clsReservationData.DeleteReservation(ReservationID);
        }


        public static List<ReservationDTO> GetAllReservations()
        {
            return clsReservationData.GetAllReservations();
        }


    }

}
