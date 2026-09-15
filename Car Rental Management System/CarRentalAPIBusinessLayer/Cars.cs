using CarRentalDataAccessLayer;
using CarRentalDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalDTOs.CarDTO;

namespace CarRentalAPIBusinessLayer
{
    public class clsCar
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;

        public enum enSaveResult
        {
            Success = 0,
            PlateNumberAlreadyUsed,
            VINAlreadyUsed,
            FuelTypeNotFound,
            CarCategoryNotFound,
            CurrentBranchNotFound,
            CurrentBranchInactive,

            DatabaseError
        }


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


        public CarDTO CDTO
        {
            get { return new CarDTO(this.CarID, this.Make, this.Model, this.Year, this.PlateNumber, this.VIN, this.Color, this.NumberOfSeats, this.TransmissionType, this.FuelTypeID, this.CarCategoryID, this.CarStatus, this.DailyRentalPrice, this.CurrentMileage, this.CurrentBranchID); }
        }


        public clsCar(CarDTO CDTO, enMode cMode = enMode.AddNew)
        {
            this.CarID = CDTO.CarID;
            this.Make = CDTO.Make;
            this.Model = CDTO.Model;
            this.Year = CDTO.Year;
            this.PlateNumber = CDTO.PlateNumber;
            this.VIN = CDTO.VIN;
            this.Color = CDTO.Color;
            this.NumberOfSeats = CDTO.NumberOfSeats;
            this.TransmissionType = CDTO.TransmissionType;
            this.FuelTypeID = CDTO.FuelTypeID;
            this.CarCategoryID = CDTO.CarCategoryID;
            this.CarStatus = CDTO.CarStatus;
            this.DailyRentalPrice = CDTO.DailyRentalPrice;
            this.CurrentMileage = CDTO.CurrentMileage;
            this.CurrentBranchID = CDTO.CurrentBranchID;

            this.Mode = cMode;
        }


        public static clsCar? Find(int CarID)
        {
            CarDTO? CDTO = clsCarData.GetCarInfoByCarID(CarID);

            if (CDTO != null)
            {
                return new clsCar(CDTO, enMode.Update);
            }
            else
                return null;
        }


        private bool _AddNewCar()
        {
            this.CarID = clsCarData.AddNewCar(CDTO);
            return (CarID != -1);
        }


        private bool _UpdateCar()
        {
            return clsCarData.UpdateCar(CDTO);
        }


        public enSaveResult Save()
        {
            short RCode = 7;

            switch (Mode)
            {
                case enMode.AddNew:

                    RCode = ValidateCarData(this.CDTO);

                    if (RCode != -1)
                    {
                        return (enSaveResult)RCode;
                    }

                    if (_AddNewCar())
                    {
                        Mode = enMode.Update;
                        return enSaveResult.Success;
                    }
                    else
                    {
                        return enSaveResult.DatabaseError;
                    }
                case enMode.Update:

                    RCode = ValidateCarDataForUpdate(this.CDTO);

                    if (RCode != -1)
                    {
                        return (enSaveResult)RCode;
                    }

                    if (_UpdateCar())
                        return enSaveResult.Success;
                    else
                        return enSaveResult.DatabaseError;

            }

            return enSaveResult.DatabaseError;
        }


        public static bool DeleteCar(int CarID)
        {
            return clsCarData.DeleteCar(CarID);
        }


        public static List<CarDTO> GetAllCars()
        {
            return clsCarData.GetAllCars();
        }

        public static bool CheckCarAvailability(int CarID,ref string message)
        {
            return clsCarData.CheckCarAvailability(CarID,ref message);
        }

        public static short ValidateCarData(CarDTO CDTO)
        { 
            return clsCarData.ValidateCarData(CDTO);
        }

        public static short ValidateCarDataForUpdate(CarDTO CDTO)
        {
            return clsCarData.ValidateCarDataForUpdate(CDTO);
        }

    }


}
