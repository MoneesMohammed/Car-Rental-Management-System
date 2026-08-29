using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalDTOs
{
    public class EmployeeRequest
    {
        public int JobTitleID { get; set; }
        public int WorkingBranchID { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }
        public PersonDTO.enGender Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string? ImagePath { get; set; }
    }
}
