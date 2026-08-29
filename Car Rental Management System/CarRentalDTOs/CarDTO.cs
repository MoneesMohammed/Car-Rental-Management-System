using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalDTOs
{
    public class CarDTO
    {
        public enum enTransmissionType { Automatic = 0, Manual = 1 };

        public enum enCarStatus { Available = 0, Rented = 1, Reserved = 2, Maintenance = 3, Retired = 4 };

        public int CarID { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string PlateNumber { get; set; }
        public string VIN { get; set; }
        public string Color { get; set; }
        public int NumberOfSeats { get; set; }
        public enTransmissionType TransmissionType { get; set; }
        public int FuelTypeID { get; set; }
        public int CarCategoryID { get; set; }
        public enCarStatus CarStatus { get; set; }
        public decimal DailyRentalPrice { get; set; }
        public int CurrentMileage { get; set; }
        public int? CurrentBranchID { get; set; }

        public CarDTO(int CarID, string Make, string Model, int Year, string PlateNumber, string VIN, string Color, int NumberOfSeats, enTransmissionType TransmissionType, int FuelTypeID, int CarCategoryID, enCarStatus CarStatus, decimal DailyRentalPrice, int CurrentMileage, int? CurrentBranchID)
        {
            this.CarID = CarID;
            this.Make = Make;
            this.Model = Model;
            this.Year = Year;
            this.PlateNumber = PlateNumber;
            this.VIN = VIN;
            this.Color = Color;
            this.NumberOfSeats = NumberOfSeats;
            this.TransmissionType = TransmissionType;
            this.FuelTypeID = FuelTypeID;
            this.CarCategoryID = CarCategoryID;
            this.CarStatus = CarStatus;
            this.DailyRentalPrice = DailyRentalPrice;
            this.CurrentMileage = CurrentMileage;
            this.CurrentBranchID = CurrentBranchID;
        }

    }

}
