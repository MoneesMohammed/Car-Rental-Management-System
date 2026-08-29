using CarRentalDataAccessLayer;
using CarRentalDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalDTOs.PaymentDTO;

namespace CarRentalAPIBusinessLayer
{
    public class clsPayment
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;

        public int PaymentID { get; set; }
        public int RentalContractID { get; set; }
        public enPaymentMethod PaymentMethod { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public enPaymentStatus PaymentStatus { get; set; }
        public string? TransactionReference { get; set; }

        public PaymentDTO PDTO
        {
            get { return new PaymentDTO(this.PaymentID, this.RentalContractID, this.PaymentMethod, this.PaymentDate, this.AmountPaid, this.PaymentStatus, this.TransactionReference); }
        }

        public clsPayment(PaymentDTO PDTO, enMode cMode = enMode.AddNew)
        {
            this.PaymentID = PDTO.PaymentID;
            this.RentalContractID = PDTO.RentalContractID;
            this.PaymentMethod = PDTO.PaymentMethod;
            this.PaymentDate = PDTO.PaymentDate;
            this.AmountPaid = PDTO.AmountPaid;
            this.PaymentStatus = PDTO.PaymentStatus;
            this.TransactionReference = PDTO.TransactionReference;

            this.Mode = cMode;
        }


        public static clsPayment? Find(int PaymentID)
        {
            PaymentDTO? PDTO = clsPaymentData.GetPaymentInfoByPaymentID(PaymentID);

            if (PDTO != null)
            {
                return new clsPayment(PDTO, enMode.Update);
            }
            else
                return null;
        }


        private bool _AddNewPayment()
        {
            this.PaymentID = clsPaymentData.AddNewPayment(PDTO);
            return (PaymentID != -1);
        }


        private bool _UpdatePayment()
        {
            return clsPaymentData.UpdatePayment(PDTO);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:

                    if (_AddNewPayment())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:

                    return (_UpdatePayment());

            }

            return false;
        }


        public static bool DeletePayment(int PaymentID)
        {
            return clsPaymentData.DeletePayment(PaymentID);
        }


        public static List<PaymentDTO> GetAllPayments()
        {
            return clsPaymentData.GetAllPayments();
        }


    }

}
