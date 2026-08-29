using CarRentalDataAccessLayer;
using CarRentalDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalAPIBusinessLayer
{
    public class clsBranch
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;


        public int BranchID { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }


        public BranchDTO BDTO
        {
            get { return new BranchDTO(this.BranchID, this.BranchName, this.Address, this.City, this.Phone, this.IsActive); }
        }

        public clsBranch(BranchDTO BDTO, enMode cMode = enMode.AddNew)
        {
            this.BranchID = BDTO.BranchID;
            this.BranchName = BDTO.BranchName;
            this.Address = BDTO.Address;
            this.City = BDTO.City;
            this.Phone = BDTO.Phone;
            this.IsActive = BDTO.IsActive;

            this.Mode = cMode;
        }


        public static clsBranch? Find(int BranchID)
        {
            BranchDTO? BDTO = clsBranchData.GetBranchInfoByBranchID(BranchID);

            if (BDTO != null)
            {
                return new clsBranch(BDTO, enMode.Update);
            }
            else
                return null;
        }


        private bool _AddNewBranch()
        {
            this.BranchID = clsBranchData.AddNewBranch(BDTO);
            return (BranchID != -1);
        }


        private bool _UpdateBranch()
        {
            return clsBranchData.UpdateBranch(BDTO);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:

                    if (_AddNewBranch())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:

                    return (_UpdateBranch());

            }

            return false;
        }


        public static bool DeleteBranch(int BranchID)
        {
            return clsBranchData.DeleteBranch(BranchID);
        }


        public static List<BranchDTO> GetAllBranches()
        {
            return clsBranchData.GetAllBranches();
        }

        public static bool ActivateBranch(int BranchID)
        {
            return clsBranchData.ActivateBranch(BranchID);
        }

        public static List<BranchDTO> GetAllDeactivateBranches()
        {
            return clsBranchData.GetAllDeactivateBranches();
        }


    }

}
