using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalDTOs
{
    public class CarCategoryDTO
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        public CarCategoryDTO(int CategoryID, string CategoryName)
        {
            this.CategoryID = CategoryID;
            this.CategoryName = CategoryName;
        }
    }

}
