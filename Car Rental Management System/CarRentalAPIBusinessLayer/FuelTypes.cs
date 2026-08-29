using CarRentalDataAccessLayer;
using CarRentalDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalAPIBusinessLayer
{
    public class clsFuelType
    {
        public int FuelTypeID { get; set; }
        public string FuelType { get; set; }

        public FuelTypeDTO FDTO
        {
            get { return new FuelTypeDTO(this.FuelTypeID, this.FuelType); }
        }

        public clsFuelType(FuelTypeDTO FDTO)
        {
            this.FuelTypeID = FDTO.FuelTypeID;
            this.FuelType = FDTO.FuelType;
        }

        public static clsFuelType? Find(int FuelTypeID)
        {
            FuelTypeDTO? FDTO = clsFuelTypeData.GetFuelTypeInfoByFuelTypeID(FuelTypeID);

            if (FDTO != null)
            {
                return new clsFuelType(FDTO);
            }
            else
                return null;
        }

        public static List<FuelTypeDTO> GetAllFuelTypes()
        {
            return clsFuelTypeData.GetAllFuelTypes();
        }

    }

}
