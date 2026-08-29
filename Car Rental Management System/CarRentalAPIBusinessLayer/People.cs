using CarRentalDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentalDTOs;

namespace CarRentalAPIBusinessLayer
{
    public class clsPerson
    {

        public int PersonID { get; set; }
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


        protected PersonDTO PDTO
        {
            get { return new PersonDTO(this.PersonID, this.FirstName, this.SecondName, this.ThirdName, this.LastName, this.DateOfBirth, this.Gender, this.Address, this.Phone, this.Email, this.ImagePath); }
        }

        public clsPerson()
        {
            this.PersonID    = 0;
            this.FirstName   = "";
            this.SecondName  = "";
            this.ThirdName   = "";
            this.LastName    = "";
            this.DateOfBirth = DateTime.Now;
            this.Gender      = 0;
            this.Address     = "";
            this.Phone       = "";
            this.Email       = "";
            this.ImagePath   = null;
        }
        

        public clsPerson(PersonDTO PDTO)
        {
            this.PersonID    = PDTO.PersonID;
            this.FirstName   = PDTO.FirstName;
            this.SecondName  = PDTO.SecondName;
            this.ThirdName   = PDTO.ThirdName;
            this.LastName    = PDTO.LastName;
            this.DateOfBirth = PDTO.DateOfBirth;
            this.Gender      = PDTO.Gender;
            this.Address     = PDTO.Address;
            this.Phone       = PDTO.Phone;
            this.Email       = PDTO.Email;
            this.ImagePath   = PDTO.ImagePath;

        }


        //public static clsPerson? Find(int PersonID)
        //{
        //    PersonDTO? PDTO = clsPersonData.GetPersonInfoByPersonID(PersonID);

        //    if (PDTO != null)
        //    {
        //        return new clsPerson(PDTO);
        //    }
        //    else
        //        return null;
        //}




    }

}
