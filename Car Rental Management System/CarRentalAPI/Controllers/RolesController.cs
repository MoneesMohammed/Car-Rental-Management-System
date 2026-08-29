using CarRentalAPIBusinessLayer;
using CarRentalDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{

    //[Route("api/[controller]")]
    [Route("api/Roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {

        [HttpGet("All", Name = "GetAllRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<RoleDTO>> GetAllRoles()
        {
            List<RoleDTO> RoleList = clsRole.GetAllRoles();

            if (RoleList.Count == 0)
                return NotFound("No Roles Found!");
            
            return Ok(RoleList);
        }


        [HttpGet("{ID}", Name = "GetRoleByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<RoleDTO> GetRoleByID(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }

            var Role = clsRole.Find(ID);

            if (Role == null)
            {
                return NotFound($"Role with ID: {ID} not found.");
            }

            RoleDTO RDTO = Role.RDTO;

            return Ok(RDTO);
        }



    }

}
