using CarRentalAPIBusinessLayer;
using CarRentalDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace CarRentalAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {

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


        [HttpGet("{ID}", Name = "GetUserByID")]
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

            NewUserDTO.PasswordHash = BCrypt.Net.BCrypt.HashPassword(NewUserDTO.PasswordHash);

            var User = new clsUser(NewUserDTO);

            if (!User.Save())
            {
                return StatusCode(500, new { Message = "Error : Adding User." });
            }

            NewUserDTO.UserID = User.UserID;
            NewUserDTO.CreateDate = DateTime.Now;

            return CreatedAtRoute("GetUserByID", new { ID = NewUserDTO.UserID }, NewUserDTO);
        }


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

            User.EmployeeID = updatedUser.EmployeeID;
            User.RoleID = updatedUser.RoleID;
            User.UserName = updatedUser.UserName;
            User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updatedUser.PasswordHash);
            User.IsActive = updatedUser.IsActive;
            

            if (!User.Save())
            {
                return StatusCode(500, new { Message = "Error : Updating User." });
            }

            return Ok(User.UDTO);
        }


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
                return Ok($"User with ID: {ID} has been Deleted.");
            }
            else
            {
                return NotFound($"User with ID {ID} not found. no rows deleted!");
            }

        }

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

            return Ok($"User with ID: {ID} has been Activated.");
        }

        //UpdateLastLoginDate

        [HttpPut("{ID}/LastLogin", Name = "UpdateLastLogin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdateLastLogin(int ID)
        {
            if (ID < 1) //you will add all validation here 
            {
                return BadRequest("Invalid User data.");
            }

            bool IsUpdate = clsUser.UpdateLastLoginDate(ID);

            if (!IsUpdate)
            {
                return NotFound($"User with ID {ID} not found , or already Active.");
            }

            return Ok($"User with ID: {ID} has been Updated to Last Login Date.");
        }


        [HttpGet("AllDeactivate", Name = "GetAllDeactivateUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<UserDTO>> GetAllDeactivateUsers()
        {
            List<UserDTO> UserList = clsUser.GetAllDeactivateUsers();

            if (UserList.Count == 0)
                return NotFound("No Users Found!");

            return Ok(UserList);
        }


    }

}
