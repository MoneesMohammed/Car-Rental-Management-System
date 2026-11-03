using CarRentalAPIBusinessLayer;
using CarRentalDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace CarRentalAPI.Controllers
{
    //[Route("api/[controller]")]
    [Authorize]
    [Route("api/Branches")]
    [ApiController]
    public class BranchesController : ControllerBase
    {

        [HttpGet("All", Name = "GetAllBranches")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<BranchDTO>> GetAllBranches()
        {
            List<BranchDTO> BrancheList = clsBranch.GetAllBranches();

            if (BrancheList.Count == 0)
                return NotFound("No Branches Found!");

            return Ok(BrancheList);
        }


        [HttpGet("{ID}", Name = "GetBranchByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<BranchDTO> GetBranchByID(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }

            var Branche = clsBranch.Find(ID);

            if (Branche == null)
            {
                return NotFound($"Branch with ID: {ID} not found.");
            }

            BranchDTO BDTO = Branche.BDTO;

            return Ok(BDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Name = "AddBranch")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<BranchDTO> AddBranch(BranchDTO NewBranchDTO)
        {
            if (NewBranchDTO == null)//you will add all validation here 
            {
                return BadRequest("Invalid Branch Data.");
            }

            var Branch = new clsBranch(NewBranchDTO);

            if (!Branch.Save())
            {
                return StatusCode(500, new { Message = "Error : Adding Branch." });
            }

            var userID = int.Parse(base.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            string NewValue = JsonSerializer.Serialize(Branch.BDTO);
            var ALDTO = new AuditLogDTO(0, userID, "AddNew", "Branch", Branch.BranchID, null, NewValue, DateTime.UtcNow);
            new clsAuditLog(ALDTO).Save();


            NewBranchDTO.BranchID = Branch.BranchID;

            return CreatedAtRoute("GetBranchByID", new { ID = NewBranchDTO.BranchID }, NewBranchDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{ID}", Name = "UpdateBranch")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<BranchDTO> UpdateBranch(int ID, BranchDTO updatedBranch)
        {
            if (ID < 1 || updatedBranch == null) //you will add all validation here 
            {
                return BadRequest("Invalid Branch data.");
            }

            var Branche = clsBranch.Find(ID);

            if (Branche == null )
            {
                return NotFound($"Branch with ID {ID} not found.");
            }

            string OldValue = JsonSerializer.Serialize(Branche.BDTO);

            Branche.BranchName = updatedBranch.BranchName;
            Branche.Address = updatedBranch.Address;
            Branche.City = updatedBranch.City;
            Branche.Phone = updatedBranch.Phone;
            
            if (!Branche.Save())
            {
                return StatusCode(500, new { Message = "Error : Updating Branch." });
            }

            string NewValue = JsonSerializer.Serialize(Branche.BDTO);
            var userID = int.Parse(base.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var ALDTO = new AuditLogDTO(0, userID, "Update", "Branche", ID, OldValue, NewValue, DateTime.UtcNow);
            new clsAuditLog(ALDTO).Save();

            return Ok(Branche.BDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{ID}", Name = "DeleteBranch")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteBranch(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }


            if (clsBranch.DeleteBranch(ID))
            {
                var userID = int.Parse(base.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var ALDTO = new AuditLogDTO(0, userID, "Delete", "Branch", ID, null, null, DateTime.UtcNow);
                new clsAuditLog(ALDTO).Save();

                return Ok($"Branch with ID: {ID} has been Deleted.");
            }
            else
            {
                return NotFound($"Branch with ID {ID} not found.Or no rows deleted!");
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{ID}/Activate", Name = "ActivateBranch")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult ActivateBranch(int ID)
        {
            if (ID < 1) //you will add all validation here 
            {
                return BadRequest("Invalid Branch data.");
            }

            bool IsActive = clsBranch.ActivateBranch(ID);

            if (!IsActive)
            {
                return NotFound($"Branch with ID {ID} not found , or already Active.");
            }

            var userID = int.Parse(base.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var ALDTO = new AuditLogDTO(0, userID, "Activated", "Branch", ID, null, null, DateTime.UtcNow);
            new clsAuditLog(ALDTO).Save();

            return Ok($"Branch with ID: {ID} has been Activated.");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Deactivated", Name = "GetDeactivatedBranches")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<BranchDTO>> GetDeactivatedBranches()
        {
            List<BranchDTO> BrancheList = clsBranch.GetAllDeactivateBranches();

            if (BrancheList.Count == 0)
                return NotFound("No Deactivated Branches Were Found!");

            return Ok(BrancheList);
        }

    }

}
