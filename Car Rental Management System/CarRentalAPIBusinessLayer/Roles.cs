using CarRentalDataAccessLayer;
using CarRentalDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalAPIBusinessLayer
{
    public class clsRole
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }


        public RoleDTO RDTO
        {
            get { return new RoleDTO(this.RoleID, this.RoleName); }
        }


        public clsRole(RoleDTO RDTO)
        {
            this.RoleID = RDTO.RoleID;
            this.RoleName = RDTO.RoleName;

            
        }


        public static clsRole? Find(int RoleID)
        {
            RoleDTO? RDTO = clsRoleData.GetRoleInfoByRoleID(RoleID);

            if (RDTO != null)
            {
                return new clsRole(RDTO);
            }
            else
                return null;
        }

        public static List<RoleDTO> GetAllRoles()
        {
            return clsRoleData.GetAllRoles();
        }

    }

}
