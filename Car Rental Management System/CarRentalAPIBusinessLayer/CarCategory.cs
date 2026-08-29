using CarRentalDataAccessLayer;
using CarRentalDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalAPIBusinessLayer
{
    public class clsCarCategory
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        public CarCategoryDTO CDTO
        {
            get { return new CarCategoryDTO(this.CategoryID, this.CategoryName); }
        }

        public clsCarCategory(CarCategoryDTO CDTO)
        {
            this.CategoryID = CDTO.CategoryID;
            this.CategoryName = CDTO.CategoryName;

        }

        public static clsCarCategory? Find(int CategoryID)
        {
            CarCategoryDTO? CDTO = clsCarCategoryData.GetCarCategoryInfoByCategoryID(CategoryID);

            if (CDTO != null)
            {
                return new clsCarCategory(CDTO);
            }
            else
                return null;
        }

        public static List<CarCategoryDTO> GetAllCarCategory()
        {
            return clsCarCategoryData.GetAllCarCategory();
        }

    }


}
