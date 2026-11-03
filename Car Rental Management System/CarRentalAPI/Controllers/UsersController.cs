using BCrypt.Net;
using CarRentalAPI.Global_Classes;
using CarRentalAPIBusinessLayer;
using CarRentalDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Security.Claims;
using System.Text.Json;

namespace CarRentalAPI.Controllers
{
    //[Route("api/[controller]")]

    [Authorize]
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet("All", Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<UserDTO>> GetAllUsers()
        {
            List<UserDTO> UserList = clsUser.GetAllUsers();

            if (UserList.Count == 0)
                return NotFound("No Users Found!");

            return Ok(UserList);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("ByID/{ID}", Name = "GetUserByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserDTO> GetUserByID(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }

            var User = clsUser.Find(ID);

            if (User == null)
            {
                return NotFound($"User with ID: {ID} not found.");
            }

            UserDTO UDTO = User.UDTO;

            return Ok(UDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("ByUserName/{UserName}", Name = "GetUserByUserName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserDTO> GetUserByUserName(string UserName)
        {
            if (string.IsNullOrEmpty(UserName))
            {
                return BadRequest($"Not Accepted UserName: Is Null Or Empty");
            }

            var User = clsUser.Find(UserName);

            if (User == null)
            {
                return NotFound($"User with UserName: {UserName} not found.");
            }

            UserDTO UDTO = User.UDTO;

            return Ok(UDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Name = "AddUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UserDTO> AddUser(UserDTO NewUserDTO)
        {
            if (NewUserDTO == null)//you will add all validation here 
            {
                return BadRequest("Invalid User Data.");
            }
            
            if (!clsValidation.IsValidPassword(NewUserDTO.PasswordHash))
            {
                return BadRequest(new
                {
                    Message = "Password does not meet the security requirements.",
                    Requirements = new[]
                    {
                        "At least 8 characters",
                        "At least one uppercase letter (A-Z)",
                        "At least one lowercase letter (a-z)",
                        "At least one number (0-9)",
                        "At least one special character (!@#$%^&*)"
                    }
                });
            }


            NewUserDTO.PasswordHash = BCrypt.Net.BCrypt.HashPassword(NewUserDTO.PasswordHash);

            var User = new clsUser(NewUserDTO);

            var result = User.Save();

            switch (result)
            {
                case clsUser.enSaveResult.Success:

                    var userID = int.Parse(base.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                    string NewValue = JsonSerializer.Serialize(User.UDTO);
                    var ALDTO = new AuditLogDTO(0, userID, "AddNew", "User", User.UserID, null, NewValue, DateTime.UtcNow);
                    new clsAuditLog(ALDTO).Save();

                    NewUserDTO.UserID = User.UserID;
                    NewUserDTO.CreateDate = DateTime.Now;       
                    NewUserDTO.RoleName = clsRole.Find(NewUserDTO.RoleID)!.RoleName ;

                    return CreatedAtRoute("GetUserByID", new { ID = NewUserDTO.UserID }, NewUserDTO);

                case clsUser.enSaveResult.RoleNotFound:
                    return BadRequest("Role not found.");

                case clsUser.enSaveResult.EmployeeNotFound:
                    return BadRequest("Employee not found.");

                case clsUser.enSaveResult.EmployeeInactive:
                    return BadRequest("Employee is inactive.");

                case clsUser.enSaveResult.EmployeeAlreadyExists:
                    return BadRequest("Employee Already Exists.");

                case clsUser.enSaveResult.UserNameAlreadyUsed:
                    return BadRequest("UserName already used.");

                default:
                    return StatusCode(500, new { Message = "Error : Adding User." });
            }


        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{ID}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UserDTO> UpdateUser(int ID, UserDTO updatedUser)
        {
            if (ID < 1 || updatedUser == null) //you will add all validation here 
            {
                return BadRequest("Invalid User data.");
            }

            var User = clsUser.Find(ID);

            if (User == null)
            {
                return NotFound($"User with ID {ID} not found.");
            }

            if (!clsValidation.IsValidPassword(updatedUser.PasswordHash))
            {
                return BadRequest(new
                {
                    Message = "Password does not meet the security requirements.",
                    Requirements = new[]
                    {
                        "At least 8 characters",
                        "At least one uppercase letter (A-Z)",
                        "At least one lowercase letter (a-z)",
                        "At least one number (0-9)",
                        "At least one special character (!@#$%^&*)"
                    }
                });
            }

            string OldValue = JsonSerializer.Serialize(User.UDTO);

            bool RoleChanged = (User.RoleID != updatedUser.RoleID);

            string OldRole = User.RoleID.ToString();

            User.RoleID = updatedUser.RoleID;
            User.UserName = updatedUser.UserName;
            User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updatedUser.PasswordHash);

            var result = User.Save();

            switch (result)
            {
                case clsUser.enSaveResult.Success:

                    string NewValue = JsonSerializer.Serialize(User.UDTO);
                    var userID = int.Parse(base.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                    var ALDTO = new AuditLogDTO(0, userID, "Update", "User", ID, OldValue, NewValue, DateTime.UtcNow);
                    new clsAuditLog(ALDTO).Save();

                    if (RoleChanged)
                    {
                        ALDTO = new AuditLogDTO(0, userID, "Update Role", "User", ID, OldRole, User.RoleID.ToString(), DateTime.UtcNow);
                        new clsAuditLog(ALDTO).Save();
                    }
                    

                    User.RoleName = clsRole.Find(updatedUser.RoleID)!.RoleName;
                    return Ok(User.UDTO);

                case clsUser.enSaveResult.RoleNotFound:
                    return BadRequest("Role not found.");

                case clsUser.enSaveResult.EmployeeNotFound:
                    return BadRequest("Employee not found.");

                case clsUser.enSaveResult.EmployeeInactive:
                    return BadRequest("Employee is inactive.");

                case clsUser.enSaveResult.EmployeeAlreadyExists:
                    return BadRequest("Employee Already Exists.");

                case clsUser.enSaveResult.UserNameAlreadyUsed:
                    return BadRequest("UserName already used.");

                default:
                    return StatusCode(500, new { Message = "Error : Updating User." });
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{ID}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteUser(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }


            if (clsUser.DeleteUser(ID))
            {
                var userID = int.Parse(base.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var ALDTO = new AuditLogDTO(0, userID, "Delete", "User", ID, null, null, DateTime.UtcNow);
                new clsAuditLog(ALDTO).Save();

                return Ok($"User with ID: {ID} has been Deleted.");
            }
            else
            {
                return NotFound($"User with ID {ID} not found. no rows deleted!");
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{ID}/Activate", Name = "ActivateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult ActivateUser(int ID)
        {
            if (ID < 1) //you will add all validation here 
            {
                return BadRequest("Invalid User data.");
            }

            bool IsActive = clsUser.ActivateUser(ID);

            if (!IsActive)
            {
                return NotFound($"User with ID {ID} not found , or already Active.");
            }

            var userID = int.Parse(base.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var ALDTO = new AuditLogDTO(0, userID, "Activated", "User", ID, null, null, DateTime.UtcNow);
            new clsAuditLog(ALDTO).Save();

            return Ok($"User with ID: {ID} has been Activated.");
        }

        ////UpdateLastLoginDate

        //[HttpPut("{ID}/LastLogin", Name = "UpdateLastLogin")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public ActionResult UpdateLastLogin(int ID)
        //{
        //    if (ID < 1) //you will add all validation here 
        //    {
        //        return BadRequest("Invalid User data.");
        //    }

        //    bool IsUpdate = clsUser.UpdateLastLoginDate(ID);

        //    if (!IsUpdate)
        //    {
        //        return NotFound($"User with ID {ID} not found , or already Active.");
        //    }

        //    return Ok($"User with ID: {ID} has been Updated to Last Login Date.");
        //}

        [Authorize(Roles = "Admin")]
        [HttpGet("Deactivated", Name = "GetDeactivatedUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<UserDTO>> GetDeactivatedUsers()
        {
            List<UserDTO> UserList = clsUser.GetAllDeactivateUsers();

            if (UserList.Count == 0)
                return NotFound("No Deactivated Users Were Found!");

            return Ok(UserList);
        }

        //[Authorize(Roles = "Admin")]//need to add Policy
        [HttpPut("{ID}/ChangePassword", Name = "ChangePassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task <ActionResult> ChangePassword(int ID, [FromBody] ChangePasswordRequest request,
            [FromServices] IAuthorizationService authorizationService)
        {
            if (ID < 1 || request == null || 
                string.IsNullOrWhiteSpace(request.CurrentPassword) || string.IsNullOrWhiteSpace(request.NewPassword)) 
            {
                return BadRequest("Invalid Password data.");
            }

            var authResult = await authorizationService.AuthorizeAsync(HttpContext.User, ID, "UserOwnPasswordPolicy");

            if (!authResult.Succeeded)
                return Forbid(); // 403


            if (request.CurrentPassword == request.NewPassword)
            {
                return BadRequest("New password must be different from current password.");
            }

            clsUser? User = clsUser.Find(ID);

            if (User == null)
            {
                return NotFound($"User with ID {ID} not found.");
            }

            if (!User.IsActive)
            {
                return Unauthorized("User Inactive");
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.CurrentPassword, User.PasswordHash);

            if (!isValidPassword)
            {
                return Unauthorized("Invalid Current Password.");
            }

            if (!clsValidation.IsValidPassword(request.NewPassword))
            {
                return BadRequest(new
                {
                    Message = "Password does not meet the security requirements.",
                    Requirements = new[]
                    {
                        "At least 8 characters",
                        "At least one uppercase letter (A-Z)",
                        "At least one lowercase letter (a-z)",
                        "At least one number (0-9)",
                        "At least one special character (!@#$%^&*)"
                    }
                });
            }

            User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            var result = User.SavePassword();

            if(!result)
                return StatusCode(500, new { Message = "Error : Changing Password." });

            var userID = int.Parse(base.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var ALDTO = new AuditLogDTO(0, userID, "Change Password", "User", ID, null, null, DateTime.UtcNow);
            new clsAuditLog(ALDTO).Save();

            return Ok(new { Message = "Password Changed Successfully." });
            
        }

    }

}
