using CarRentalAPI.Authorization;
using CarRentalAPI.Global_Classes;
using CarRentalAPIBusinessLayer;
using CarRentalDataAccessLayer;
using CarRentalDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Text.Json;

namespace CarRentalAPI.Controllers
{
    //[Route("api/[controller]")]
    [Authorize]
    [Route("api/Employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        private readonly ILogger<EmployeesController> _logger;
        public EmployeesController(ILogger<EmployeesController> logger)
        {
            _logger = logger;
        }


        [Authorize(Roles = "Admin,Manager")]
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

        // [Authorize(Roles = "Admin,Manager")]//That Need Ownership, Rental Agent (Owner Only)
        [HttpGet("{ID}", Name = "GetEmployeeByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task <ActionResult<EmployeeDTO>> GetEmployeeByID(int ID ,
            [FromServices] IAuthorizationService authorizationService)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }

            var authResult = await authorizationService.AuthorizeAsync(User,ID, "EmployeeDataPolicy");

            if (!authResult.Succeeded)
                return Forbid(); // 403


            var Employee = clsEmployee.Find(ID);

            if (Employee == null)
            {
                return NotFound($"Employee with ID: {ID} not found.");
            }

            EmployeeDTO EDTO = Employee.EDTO;

            return Ok(EDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Name = "AddEmployee")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<EmployeeDTO> AddEmployee(EmployeeDTO NewEmployeeDTO)
        {
            if (NewEmployeeDTO == null || string.IsNullOrEmpty(NewEmployeeDTO.PDTO.Email))//you will add all validation here 
            {
                return BadRequest("Invalid Employee Data.");
            }

            var userID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (!clsValidation.ValidateEmail(NewEmployeeDTO.PDTO.Email))
            {
                return BadRequest("Invalid Email Address.");
            }


            var Employee = new clsEmployee(NewEmployeeDTO);

            var result = Employee.Save();

            switch (result)
            {
                case clsEmployee.enSaveResult.Success:
                    string NewValue = JsonSerializer.Serialize(Employee.EDTO);
                    var ALDTO = new AuditLogDTO(0, userID, "AddNew", "Employee", Employee.EmployeeID, null, NewValue, DateTime.UtcNow);
                    new clsAuditLog(ALDTO).Save();

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

        //[Authorize(Roles = "Admin")]
        [HttpPut("{ID}", Name = "UpdateEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<clsEmployee>> UpdateEmployee(int ID, EmployeeDTO updatedEmployee,
           [FromServices] IAuthorizationService authorizationService)
        {
            if (ID < 1 || updatedEmployee == null) //you will add all validation here 
            {
                return BadRequest("Invalid Employee data.");
            }

            var userID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var authResult = await authorizationService.AuthorizeAsync(User, ID, "EmployeeOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            if (!clsValidation.ValidateEmail(updatedEmployee.PDTO.Email))
            {
                return BadRequest("Invalid Email Address.");
            }

            var Employee = clsEmployee.Find(ID);

            if (Employee == null)
            {
                return NotFound($"Employee with ID {ID} not found.");
            }

            string OldValue = JsonSerializer.Serialize(Employee.EDTO);
            

            Employee.JobTitleID      = updatedEmployee.JobTitleID;
            Employee.WorkingBranchID = updatedEmployee.WorkingBranchID;
            Employee.HireDate        = updatedEmployee.HireDate;
    
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

                    string NewValue = JsonSerializer.Serialize(Employee.EDTO);
                    var ALDTO = new AuditLogDTO(0, userID, "Update", "Employee", ID, OldValue, NewValue, DateTime.UtcNow);
                    new clsAuditLog(ALDTO).Save();
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
                    return StatusCode(500, "Error : Updating employee.");
            }

            
        }

        [Authorize(Roles = "Admin")]
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

            //var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (clsEmployee.DeleteEmployee(ID))
            {
                var ALDTO = new AuditLogDTO(0, userID, "Delete", "Employee", ID, null, null, DateTime.UtcNow);
                new clsAuditLog(ALDTO).Save();

                return Ok($"Employee with ID: {ID} has been Deleted.");
            }
            else
            {
                return NotFound($"Employee with ID {ID} not found. no rows deleted!");
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{ID}/Activate", Name = "ActivateEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult ActivateEmployee(int ID)
        {
            if (ID < 1) //you will add all validation here 
            {
                return BadRequest("Invalid Employee data.");
            }

            var userID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            bool IsActive = clsEmployee.ActivateEmployee(ID);

            if (!IsActive)
            {
                return NotFound($"Employee with ID {ID} not found , or already Active.");
            }

            var ALDTO = new AuditLogDTO(0, userID, "Activated", "Employee" , ID, null, null, DateTime.UtcNow);
            new clsAuditLog(ALDTO).Save();
            
            return Ok($"Employee with ID: {ID} has been Activated.");
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("WithoutUserAccounts", Name = "WithoutUserAccounts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<EmployeeDTO>> GetEmployeesWithoutUserAccounts()
        {
            List<EmployeeDTO> EmployeeList = clsEmployee.GetEmployeesWithoutUserAccounts();

            if (EmployeeList.Count == 0)
                return NotFound("No Employees Found!");

            return Ok(EmployeeList);
        }


    }

}
