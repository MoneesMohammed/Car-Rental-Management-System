using CarRentalDataAccessLayer;
using CarRentalDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalDTOs.MaintenanceDTO;

namespace CarRentalAPIBusinessLayer
{
    public class clsMaintenance
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;

        public int MaintenanceID { get; set; }
        public int CarID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public enMaintenanceType MaintenanceType { get; set; }
        public string ProblemDescription { get; set; }
        public decimal Cost { get; set; }
        public enMaintenanceStatus MaintenanceStatus { get; set; }


        public MaintenanceDTO MDTO
        {
            get { return new MaintenanceDTO(this.MaintenanceID, this.CarID, this.StartDate, this.EndDate, this.MaintenanceType, this.ProblemDescription, this.Cost, this.MaintenanceStatus); }
        }


        public clsMaintenance(MaintenanceDTO MDTO, enMode cMode = enMode.AddNew)
        {
            this.MaintenanceID = MDTO.MaintenanceID;
            this.CarID = MDTO.CarID;
            this.StartDate = MDTO.StartDate;
            this.EndDate = MDTO.EndDate;
            this.MaintenanceType = MDTO.MaintenanceType;
            this.ProblemDescription = MDTO.ProblemDescription;
            this.Cost = MDTO.Cost;
            this.MaintenanceStatus = MDTO.MaintenanceStatus;

            this.Mode = cMode;
        }


        public static clsMaintenance? Find(int MaintenanceID)
        {
            MaintenanceDTO? MDTO = clsMaintenanceData.GetMaintenanceInfoByMaintenanceID(MaintenanceID);

            if (MDTO != null)
            {
                return new clsMaintenance(MDTO, enMode.Update);
            }
            else
                return null;
        }


        private bool _AddNewMaintenance()
        {
            this.MaintenanceID = clsMaintenanceData.AddNewMaintenance(MDTO);
            return (MaintenanceID != -1);
        }


        private bool _UpdateMaintenance()
        {
            return clsMaintenanceData.UpdateMaintenance(MDTO);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:

                    if (_AddNewMaintenance())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:

                    return (_UpdateMaintenance());

            }

            return false;
        }


        public static bool DeleteMaintenance(int MaintenanceID)
        {
            return clsMaintenanceData.DeleteMaintenance(MaintenanceID);
        }


        public static List<MaintenanceDTO> GetAllMaintenance()
        {
            return clsMaintenanceData.GetAllMaintenance();
        }


    }

}
