using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalDTOs
{
    public class MaintenanceDTO
    {

        public enum enMaintenanceType { OilChange = 0, BrakeRepair = 1, EngineRepair = 2, TireReplacement = 3, AccidentRepair = 4 };
        public enum enMaintenanceStatus { InProgress = 0, Completed = 1 };
        public int MaintenanceID { get; set; }
        public int CarID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public enMaintenanceType MaintenanceType { get; set; }
        public string ProblemDescription { get; set; }
        public decimal Cost { get; set; }
        public enMaintenanceStatus MaintenanceStatus { get; set; }

        public MaintenanceDTO(int MaintenanceID, int CarID, DateTime StartDate, DateTime? EndDate, enMaintenanceType MaintenanceType, string ProblemDescription, decimal Cost, enMaintenanceStatus MaintenanceStatus)
        {
            this.MaintenanceID = MaintenanceID;
            this.CarID = CarID;
            this.StartDate = StartDate;
            this.EndDate = EndDate;
            this.MaintenanceType = MaintenanceType;
            this.ProblemDescription = ProblemDescription;
            this.Cost = Cost;
            this.MaintenanceStatus = MaintenanceStatus;
        }
    }

}
