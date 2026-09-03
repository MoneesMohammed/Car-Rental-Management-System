using CarRentalAPIBusinessLayer;
using CarRentalDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    //[Route("api/[controller]")]
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

            NewBranchDTO.BranchID = Branch.BranchID;

            return CreatedAtRoute("GetBranchByID", new { ID = NewBranchDTO.BranchID }, NewBranchDTO);
        }


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

            if (Branche == null)
            {
                return NotFound($"Branch with ID {ID} not found.");
            }

            Branche.BranchName = updatedBranch.BranchName;
            Branche.Address = updatedBranch.Address;
            Branche.City = updatedBranch.City;
            Branche.Phone = updatedBranch.Phone;
            Branche.IsActive = updatedBranch.IsActive;

            if (!Branche.Save())
            {
                return StatusCode(500, new { Message = "Error : Updating Branch." });
            }

            return Ok(Branche.BDTO);
        }


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
                return Ok($"Branch with ID: {ID} has been Deleted.");
            }
            else
            {
                return NotFound($"Branch with ID {ID} not found. no rows deleted!");
            }

        }


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

            return Ok($"Branch with ID: {ID} has been Activated.");
        }

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
