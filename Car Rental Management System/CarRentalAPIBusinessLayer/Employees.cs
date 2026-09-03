using CarRentalDataAccessLayer;
using CarRentalDTOs;

namespace CarRentalAPIBusinessLayer
{
    public class clsEmployee : clsPerson
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;

        public enum enSaveResult
        {
            Success = 0,
            JobTitleNotFound,
            BranchNotFound,
            BranchInactive,
            EmailAlreadyUsed,
            DatabaseError
        }

        public int EmployeeID { get; set; }
        public int JobTitleID { get; set; }
        public int WorkingBranchID { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }

        public string OldEmail { get;}

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

            this.OldEmail = EDTO.PDTO.Email;


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
            int PersonID = -1;
            this.EmployeeID = clsEmployeeData.AddNewEmployee(EDTO ,ref PersonID);

            if (EmployeeID != -1)
                this.PersonID = PersonID;

            return (EmployeeID != -1);
        }


        private bool _UpdateEmployee()
        {
            return clsEmployeeData.UpdateEmployee(EDTO);
        }


        public enSaveResult Save()
        {
            if (clsJobTitle.Find(this.JobTitleID) == null)
                return enSaveResult.JobTitleNotFound;

            var branch = clsBranch.Find(this.WorkingBranchID);

            if (branch == null)
                return enSaveResult.BranchNotFound;

            if (!branch.IsActive)
                return enSaveResult.BranchInactive;

            switch (Mode)
            {
                case enMode.AddNew:

                    if (!IsEmailUnique(this.Email))
                        return enSaveResult.EmailAlreadyUsed;

                    if (_AddNewEmployee())
                    {
                        Mode = enMode.Update;
                        return enSaveResult.Success;
                    }
                    else
                    {
                        return enSaveResult.DatabaseError;
                    }

                case enMode.Update:

                    if (this.Email != this.OldEmail)
                    {
                        if (!IsEmailUnique(this.Email))
                            return enSaveResult.EmailAlreadyUsed;
                    }

                    if (_UpdateEmployee())
                    { 
                        return enSaveResult.Success;
                    }
                    else
                    {
                        return enSaveResult.DatabaseError;
                    }


            }

            return enSaveResult.DatabaseError;
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
