using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalDTOs
{
    public class BranchDTO
    {
        public int BranchID { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }

        public BranchDTO(int BranchID, string BranchName, string Address, string City, string Phone, bool IsActive)
        {
            this.BranchID = BranchID;
            this.BranchName = BranchName;
            this.Address = Address;
            this.City = City;
            this.Phone = Phone;
            this.IsActive = IsActive;
        }
    }
}
