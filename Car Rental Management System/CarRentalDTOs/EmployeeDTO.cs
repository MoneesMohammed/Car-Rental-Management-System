namespace CarRentalDTOs
{
    public class EmployeeDTO
    {
        public int EmployeeID { get; set; }
       // public int PersonID { get; set; }
        public int JobTitleID { get; set; }
        public int WorkingBranchID { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }

        public PersonDTO PDTO { get; set; }

        public EmployeeDTO(int EmployeeID, int JobTitleID, int WorkingBranchID, DateTime HireDate, bool IsActive, PersonDTO PDTO)
        {
            this.EmployeeID = EmployeeID;
            //this.PersonID = PersonID;
            this.JobTitleID = JobTitleID;
            this.WorkingBranchID = WorkingBranchID;
            this.HireDate = HireDate;
            this.IsActive = IsActive;
            this.PDTO = PDTO;


        }
    }
}
