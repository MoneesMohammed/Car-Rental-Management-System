using CarRentalDataAccessLayer;
using CarRentalDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalAPIBusinessLayer.clsEmployee;

namespace CarRentalAPIBusinessLayer
{
    public class clsCustomer : clsPerson
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;

        public enum enSaveResult
        {
            Success = 0,
            DLNoAlreadyUsed,
            EmailAlreadyUsed,
            ExpiredDriverlicense,
            DatabaseError
        }


        public int CustomerID { get; set; }
        public string DrivingLicenseNo { get; set; }
        public DateTime DrivingLicenseExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }

        public string OldEmail { get; }
        public string OldDrivingLicenseNo { get; }


        public CustomerDTO CDTO
        {
            get { return new CustomerDTO(this.CustomerID, this.DrivingLicenseNo, this.DrivingLicenseExpiryDate, this.IsActive, this.CreateDate , base.PDTO); }
        }


        public clsCustomer(CustomerDTO CDTO, enMode cMode = enMode.AddNew)
        {
            this.CustomerID = CDTO.CustomerID;
            this.DrivingLicenseNo = CDTO.DrivingLicenseNo;
            this.DrivingLicenseExpiryDate = CDTO.DrivingLicenseExpiryDate;
            this.IsActive = CDTO.IsActive;
            this.CreateDate = CDTO.CreateDate;

            base.PersonID    = CDTO.PDTO.PersonID;
            base.FirstName   = CDTO.PDTO.FirstName;
            base.SecondName  = CDTO.PDTO.SecondName;
            base.ThirdName   = CDTO.PDTO.ThirdName;
            base.LastName    = CDTO.PDTO.LastName;
            base.DateOfBirth = CDTO.PDTO.DateOfBirth;
            base.Gender      = CDTO.PDTO.Gender;
            base.Address     = CDTO.PDTO.Address;
            base.Phone       = CDTO.PDTO.Phone;
            base.Email       = CDTO.PDTO.Email;
            base.ImagePath   = CDTO.PDTO.ImagePath;

            this.OldEmail = CDTO.PDTO.Email;
            this.OldDrivingLicenseNo = CDTO.DrivingLicenseNo;

            this.Mode = cMode;
        }

        public static clsCustomer? Find(int CustomerID)
        {
            CustomerDTO? CDTO = clsCustomerData.GetCustomerInfoByCustomerID(CustomerID);

            if (CDTO != null)
            {
                return new clsCustomer(CDTO, enMode.Update);
            }
            else
                return null;
        }


        private bool _AddNewCustomer()
        {
            int PersonID = -1;

            this.CustomerID = clsCustomerData.AddNewCustomer(CDTO ,ref PersonID);

            if (CustomerID != -1)
                this.PersonID = PersonID;

            return (CustomerID != -1);
        }


        private bool _UpdateCustomer()
        {
            return clsCustomerData.UpdateCustomer(CDTO);
        }


        public enSaveResult Save()
        {
            if (DrivingLicenseExpiryDate.Date <= DateTime.UtcNow.Date)
                return enSaveResult.ExpiredDriverlicense;


            switch (Mode)
            {
                case enMode.AddNew:

                    if (!IsDrivingLicenseNoUnique(DrivingLicenseNo))
                        return enSaveResult.DLNoAlreadyUsed;

                    if (!IsEmailUnique(this.Email))
                        return enSaveResult.EmailAlreadyUsed;

                    if (_AddNewCustomer())
                    {
                        Mode = enMode.Update;
                        return enSaveResult.Success;
                    }
                    else
                    {
                        return enSaveResult.DatabaseError;
                    }
                case enMode.Update:

                    if (this.DrivingLicenseNo != this.OldDrivingLicenseNo)
                    {
                        if (!IsDrivingLicenseNoUnique(DrivingLicenseNo))
                            return enSaveResult.DLNoAlreadyUsed;
                    }

                    if (this.Email != this.OldEmail)
                    {
                        if (!IsEmailUnique(this.Email))
                            return enSaveResult.EmailAlreadyUsed;
                    }

                    if (_UpdateCustomer())
                    {
                        return enSaveResult.Success;
                    }
                    else
                    {
                        return enSaveResult.DatabaseError;
                    }

            }


            return enSaveResult.DatabaseError;
        }


        public static bool DeleteCustomer(int CustomerID)
        {
            return clsCustomerData.DeleteCustomer(CustomerID);
        }


        public static List<CustomerDTO> GetAllCustomers()
        {
            return clsCustomerData.GetAllCustomers();
        }

        public static bool ActivateCustomer(int CustomerID)
        {
            return clsCustomerData.ActivateCustomer(CustomerID);
        }

        public static List<CustomerDTO> GetAllDeactivateCustomers()
        {
            return clsCustomerData.GetAllDeactivateCustomers();
        }

        public static bool IsDrivingLicenseNoUnique(string DrivingLicenseNo)
        {
            return clsCustomerData.IsDrivingLicenseNoUnique(DrivingLicenseNo);
        }

    }

}
