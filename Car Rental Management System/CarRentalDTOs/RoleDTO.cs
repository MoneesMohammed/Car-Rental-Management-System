using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalDTOs
{
    public class RoleDTO
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }

        public RoleDTO(int RoleID, string RoleName)
        {
            this.RoleID = RoleID;
            this.RoleName = RoleName;
        }
    }

}
