using CarRentalDataAccessLayer;
using CarRentalDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalDTOs.RentalContractDTO;

namespace CarRentalAPIBusinessLayer
{
    public class clsRentalContract
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;

        public enum enSaveResult
        {
            Success = 0,

            ReservationNotFound,
            ReservationStatusInvalid,
            ReservationAlreadyUsed,

            CarNotAvailable,

            ActualPickupDateTimeInvalid,
            OdometerAtPickupInvalid,
            FuelLevelUponReceiptInvalid,

            DiscountAmountInvalid,
            AdditionalFeesAmountInvalid,

            ContractNumberAlreadyUsed,

            DatabaseError
        }

        public int RentalContractID { get; set; }
        public string ContractNumber { get; set; }
        public int ReservationID { get; set; }
        public DateTime ActualPickupDateTime { get; set; }
        public DateTime? ActualReturnDateTime { get; set; }
        public int OdometerAtPickup { get; set; }
        public int? OdometerAtReturn { get; set; }
        public byte FuelLevelUponReceipt { get; set; }
        public byte? FuelLevelUponReturn { get; set; }
        public decimal BasicPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal AdditionalFeesAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public enContractStatus ContractStatus { get; set; }


        public RentalContractDTO RCDTO
        {
            get { return new RentalContractDTO(this.RentalContractID, this.ContractNumber, this.ReservationID, this.ActualPickupDateTime, this.ActualReturnDateTime, this.OdometerAtPickup, this.OdometerAtReturn, this.FuelLevelUponReceipt, this.FuelLevelUponReturn, this.BasicPrice, this.DiscountAmount, this.AdditionalFeesAmount, this.TotalAmount, this.ContractStatus); }
        }


        public clsRentalContract(RentalContractDTO RCDTO, enMode cMode = enMode.AddNew)
        {
            this.RentalContractID = RCDTO.RentalContractID;
            this.ContractNumber = RCDTO.ContractNumber;
            this.ReservationID = RCDTO.ReservationID;
            this.ActualPickupDateTime = RCDTO.ActualPickupDateTime;
            this.ActualReturnDateTime = RCDTO.ActualReturnDateTime;
            this.OdometerAtPickup = RCDTO.OdometerAtPickup;
            this.OdometerAtReturn = RCDTO.OdometerAtReturn;
            this.FuelLevelUponReceipt = RCDTO.FuelLevelUponReceipt;
            this.FuelLevelUponReturn = RCDTO.FuelLevelUponReturn;
            this.BasicPrice = RCDTO.BasicPrice;
            this.DiscountAmount = RCDTO.DiscountAmount;
            this.AdditionalFeesAmount = RCDTO.AdditionalFeesAmount;
            this.TotalAmount = RCDTO.TotalAmount;
            this.ContractStatus = RCDTO.ContractStatus;

            this.Mode = cMode;
        }


        public static clsRentalContract? Find(int RentalContractID)
        {
            RentalContractDTO? RCDTO = clsRentalContractData.GetRentalContractInfoByRentalContractID(RentalContractID);

            if (RCDTO != null)
            {
                return new clsRentalContract(RCDTO, enMode.Update);
            }
            else
                return null;
        }


        private bool _AddNewRentalContract()
        {
            this.RentalContractID = clsRentalContractData.AddNewRentalContract(RCDTO);
            return (RentalContractID != -1);
        }


        private bool _UpdateRentalContract()
        {
            return clsRentalContractData.UpdateRentalContract(RCDTO);
        }


        public enSaveResult Save()
        {
            short RCode = ValidateRentalContractData(this.RCDTO);

            if (RCode != -1)
            {
                return (enSaveResult)RCode;
            }


            switch (Mode)
            {
                case enMode.AddNew:

                    if (_AddNewRentalContract())
                    {
                        Mode = enMode.Update;
                        return enSaveResult.Success;
                    }
                    else
                    {
                        return enSaveResult.DatabaseError;
                    }
                case enMode.Update:

                    if (_UpdateRentalContract())
                        return enSaveResult.Success;
                    else
                        return enSaveResult.DatabaseError;

            }

            return enSaveResult.DatabaseError;
        }


        public static bool DeleteRentalContract(int RentalContractID)
        {
            return clsRentalContractData.DeleteRentalContract(RentalContractID);
        }


        public static List<RentalContractDTO> GetAllRentalContracts()
        {
            return clsRentalContractData.GetAllRentalContracts();
        }

        public short ValidateRentalContractData(RentalContractDTO RCDTO)
        {
            return clsRentalContractData.ValidateRentalContractData(RCDTO, (byte)Mode);
        }

        public static PaymentSummaryDTO? GetPaymentSummaryByID(int RentalContractID)
        {
            PaymentSummaryDTO? PSDTO = clsRentalContractData.GetPaymentSummaryByID(RentalContractID);

            if (PSDTO != null)
            {
                return PSDTO;
            }
            else
                return null;
        }

    }

}
