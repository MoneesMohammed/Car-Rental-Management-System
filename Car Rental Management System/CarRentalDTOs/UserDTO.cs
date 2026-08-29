using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalDTOs
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public int EmployeeID { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastLogin { get; set; }

        public UserDTO(int UserID, int EmployeeID, int RoleID, string RoleName ,string UserName, string PasswordHash, bool IsActive, DateTime CreateDate, DateTime? LastLogin)
        {
            this.UserID = UserID;
            this.EmployeeID = EmployeeID;
            this.RoleID = RoleID;
            this.RoleName = RoleName;
            this.UserName = UserName;
            this.PasswordHash = PasswordHash;
            this.IsActive = IsActive;
            this.CreateDate = CreateDate;
            this.LastLogin = LastLogin;
        }
    }
}
