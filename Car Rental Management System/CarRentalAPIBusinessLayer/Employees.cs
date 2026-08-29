using CarRentalDataAccessLayer;
using CarRentalDTOs;

namespace CarRentalAPIBusinessLayer
{
    public class clsEmployee : clsPerson
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;

        public int EmployeeID { get; set; }
        public int JobTitleID { get; set; }
        public int WorkingBranchID { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }


        public EmployeeDTO EDTO
        {
            get { return new EmployeeDTO(this.EmployeeID, this.JobTitleID, this.WorkingBranchID, this.HireDate, this.IsActive , this.PDTO); }
        }


        public clsEmployee(EmployeeDTO EDTO, enMode cMode = enMode.AddNew)
        {
            this.EmployeeID = EDTO.EmployeeID;
            this.JobTitleID = EDTO.JobTitleID;
            this.WorkingBranchID = EDTO.WorkingBranchID;
            this.HireDate = EDTO.HireDate;
            this.IsActive = EDTO.IsActive;

            this.PersonID    = EDTO.PDTO.PersonID;
            this.FirstName   = EDTO.PDTO.FirstName;
            this.SecondName  = EDTO.PDTO.SecondName;
            this.ThirdName   = EDTO.PDTO.ThirdName;
            this.LastName    = EDTO.PDTO.LastName;
            this.DateOfBirth = EDTO.PDTO.DateOfBirth;
            this.Gender      = EDTO.PDTO.Gender;
            this.Address     = EDTO.PDTO.Address;
            this.Phone       = EDTO.PDTO.Phone;
            this.Email       = EDTO.PDTO.Email;
            this.ImagePath   = EDTO.PDTO.ImagePath;


            this.Mode = cMode;
        }


        public static clsEmployee? Find(int EmployeeID)
        {
            EmployeeDTO? EDTO = clsEmployeeData.GetEmployeeInfoByEmployeeID(EmployeeID);

            if (EDTO != null)
            {
                return new clsEmployee(EDTO, enMode.Update);
            }
            else
                return null;
        }


        private bool _AddNewEmployee()
        {
            this.EmployeeID = clsEmployeeData.AddNewEmployee(EDTO);

            if (EmployeeID != -1)
                this.PersonID = Find(this.EmployeeID)?.PersonID ?? -1;

            return (EmployeeID != -1);
        }


        private bool _UpdateEmployee()
        {
            return clsEmployeeData.UpdateEmployee(EDTO);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:

                    if (_AddNewEmployee())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:

                    return (_UpdateEmployee());

            }

            return false;
        }


        public static bool DeleteEmployee(int EmployeeID)
        {
            return clsEmployeeData.DeleteEmployee(EmployeeID);
        }


        public static List<EmployeeDTO> GetAllEmployees()
        {
            return clsEmployeeData.GetAllEmployees();
        }


        public static bool ActivateEmployee(int EmployeeID)
        {
            return clsEmployeeData.ActivateEmployee(EmployeeID);
        }

        public static List<EmployeeDTO> GetAllDeactivateEmployees()
        {
            return clsEmployeeData.GetAllDeactivateEmployees();
        }
    }

}
