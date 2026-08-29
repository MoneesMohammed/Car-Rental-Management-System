using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalDTOs
{
    public class CustomerDTO
    {
        public int CustomerID { get; set; }
        //public int PersonID { get; set; }
        public string DrivingLicenseNo { get; set; }
        public DateTime DrivingLicenseExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }

        public PersonDTO PDTO { get; set; }

        public CustomerDTO(int CustomerID, string DrivingLicenseNo, DateTime DrivingLicenseExpiryDate, bool IsActive, DateTime CreateDate , PersonDTO PDTO)
        {
            this.CustomerID = CustomerID;
            this.DrivingLicenseNo = DrivingLicenseNo;
            this.DrivingLicenseExpiryDate = DrivingLicenseExpiryDate;
            this.IsActive = IsActive;
            this.CreateDate = CreateDate;

            this.PDTO = PDTO;
        }
    }
}
