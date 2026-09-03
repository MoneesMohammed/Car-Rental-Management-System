using CarRentalAPIBusinessLayer;
using CarRentalDataAccessLayer;
using CarRentalDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CarRentalAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/Employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        [HttpGet("All", Name = "GetAllEmployees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<EmployeeDTO>> GetAllEmployees()
        {
            List<EmployeeDTO> EmployeeList = clsEmployee.GetAllEmployees();

            if (EmployeeList.Count == 0)
                return NotFound("No Employees Found!");

            return Ok(EmployeeList);
        }


        [HttpGet("{ID}", Name = "GetEmployeeByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<EmployeeDTO> GetEmployeeByID(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }

            var Employee = clsEmployee.Find(ID);

            if (Employee == null)
            {
                return NotFound($"Employee with ID: {ID} not found.");
            }

            EmployeeDTO EDTO = Employee.EDTO;

            return Ok(EDTO);
        }


        [HttpPost(Name = "AddEmployee")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<EmployeeDTO> AddEmployee(EmployeeDTO NewEmployeeDTO)
        {
            if (NewEmployeeDTO == null)//you will add all validation here 
            {
                return BadRequest("Invalid Employee Data.");
            }

            var Employee = new clsEmployee(NewEmployeeDTO);


            var result = Employee.Save();

            switch (result)
            {
                case clsEmployee.enSaveResult.Success:
                    NewEmployeeDTO.EmployeeID = Employee.EmployeeID;
                    return CreatedAtRoute("GetEmployeeByID",new { ID = Employee.EmployeeID },Employee.EDTO);
                        
                case clsEmployee.enSaveResult.JobTitleNotFound:
                    return BadRequest("Job Title not found.");

                case clsEmployee.enSaveResult.BranchNotFound:
                    return BadRequest("Branch not found.");

                case clsEmployee.enSaveResult.BranchInactive:
                    return BadRequest("Branch is inactive.");

                case clsEmployee.enSaveResult.EmailAlreadyUsed:
                    return BadRequest("Email already used.");

                default:
                    return StatusCode(500, "Error adding employee.");
            }

        }


        [HttpPut("{ID}", Name = "UpdateEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<clsEmployee> UpdateEmployee(int ID, EmployeeDTO updatedEmployee)
        {
            if (ID < 1 || updatedEmployee == null) //you will add all validation here 
            {
                return BadRequest("Invalid Employee data.");
            }

            var Employee = clsEmployee.Find(ID);

            if (Employee == null)
            {
                return NotFound($"Employee with ID {ID} not found.");
            }



            Employee.JobTitleID      = updatedEmployee.JobTitleID;
            Employee.WorkingBranchID = updatedEmployee.WorkingBranchID;
            Employee.HireDate        = updatedEmployee.HireDate;
            Employee.IsActive        = updatedEmployee.IsActive;

            Employee.FirstName   = updatedEmployee.PDTO.FirstName;
            Employee.SecondName  = updatedEmployee.PDTO.SecondName;
            Employee.ThirdName   = updatedEmployee.PDTO.ThirdName;
            Employee.LastName    = updatedEmployee.PDTO.LastName;
            Employee.DateOfBirth = updatedEmployee.PDTO.DateOfBirth;
            Employee.Gender      = updatedEmployee.PDTO.Gender;
            Employee.Address     = updatedEmployee.PDTO.Address;
            Employee.Phone       = updatedEmployee.PDTO.Phone;
            Employee.Email       = updatedEmployee.PDTO.Email;
            Employee.ImagePath   = updatedEmployee.PDTO.ImagePath;




            var result = Employee.Save();

            switch (result)
            {
                case clsEmployee.enSaveResult.Success:
                    return Ok(Employee.EDTO);

                case clsEmployee.enSaveResult.JobTitleNotFound:
                    return BadRequest("Job Title not found.");

                case clsEmployee.enSaveResult.BranchNotFound:
                    return BadRequest("Branch not found.");

                case clsEmployee.enSaveResult.BranchInactive:
                    return BadRequest("Branch is inactive.");

                case clsEmployee.enSaveResult.EmailAlreadyUsed:
                    return BadRequest("Email already used.");

                default:
                    return StatusCode(500, "Error adding employee.");
            }

            
        }


        [HttpDelete("{ID}", Name = "DeleteEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteEmployee(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }


            if (clsEmployee.DeleteEmployee(ID))
            {
                return Ok($"Employee with ID: {ID} has been Deleted.");
            }
            else
            {
                return NotFound($"Employee with ID {ID} not found. no rows deleted!");
            }

        }

        [HttpPut("{ID}/Activate", Name = "ActivateEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult ActivateEmployee(int ID)
        {
            if (ID < 1) //you will add all validation here 
            {
                return BadRequest("Invalid Employee data.");
            }

            bool IsActive = clsEmployee.ActivateEmployee(ID);

            if (!IsActive)
            {
                return NotFound($"Employee with ID {ID} not found , or already Active.");
            }

            return Ok($"Employee with ID: {ID} has been Activated.");
        }


        [HttpGet("Deactivated", Name = "GetDeactivatedEmployees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<EmployeeDTO>> GetDeactivatedEmployees()
        {
            List<EmployeeDTO> EmployeeList = clsEmployee.GetAllDeactivateEmployees();

            if (EmployeeList.Count == 0)
                return NotFound("No Deactivated Employees Were Found!");

            return Ok(EmployeeList);
        }


    }

}
