using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalDTOs
{
    public class FuelTypeDTO
    {
        public int FuelTypeID { get; set; }
        public string FuelType { get; set; }

        public FuelTypeDTO(int FuelTypeID, string FuelType)
        {
            this.FuelTypeID = FuelTypeID;
            this.FuelType = FuelType;
        }
    }

}
