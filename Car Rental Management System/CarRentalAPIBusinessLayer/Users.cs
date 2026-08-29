using CarRentalDataAccessLayer;
using CarRentalDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalAPIBusinessLayer
{
    public class clsUser
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;

        public int UserID { get; set; }
        public int EmployeeID { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastLogin { get; set; }


        public UserDTO UDTO
        {
            get { return new UserDTO(this.UserID, this.EmployeeID, this.RoleID,this.RoleName , this.UserName, this.PasswordHash, this.IsActive, this.CreateDate, this.LastLogin); }
        }


        public clsUser(UserDTO UDTO, enMode cMode = enMode.AddNew)
        {
            this.UserID = UDTO.UserID;
            this.EmployeeID = UDTO.EmployeeID;
            this.RoleID = UDTO.RoleID;
            this.RoleName = UDTO.RoleName;
            this.UserName = UDTO.UserName;
            this.PasswordHash = UDTO.PasswordHash;
            this.IsActive = UDTO.IsActive;
            this.CreateDate = UDTO.CreateDate;
            this.LastLogin = UDTO.LastLogin;

            this.Mode = cMode;
        }


        public static clsUser? Find(int UserID)
        {
            UserDTO? UDTO = clsUserData.GetUserInfoByUserID(UserID);

            if (UDTO != null)
            {
                return new clsUser(UDTO, enMode.Update);
            }
            else
                return null;
        }


        public static clsUser? Find(string UserName)
        {
            UserDTO? UDTO = clsUserData.GetUserInfoByUserName(UserName);

            if (UDTO != null)
            {
                return new clsUser(UDTO, enMode.Update);
            }
            else
                return null;
        }

        private bool _AddNewUser()
        {
            this.UserID = clsUserData.AddNewUser(UDTO);
            return (UserID != -1);
        }


        private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(UDTO);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:

                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:

                    return (_UpdateUser());

            }

            return false;
        }


        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID);
        }


        public static List<UserDTO> GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }

        public static bool ActivateUser(int UserID)
        {
            return clsUserData.ActivateUser(UserID);
        }

        public static bool UpdateLastLoginDate(int UserID)
        {
            return clsUserData.UpdateLastLoginDate(UserID);
        }


        public static List<UserDTO> GetAllDeactivateUsers()
        {
            return clsUserData.GetAllDeactivateUsers();
        }

    }

}
