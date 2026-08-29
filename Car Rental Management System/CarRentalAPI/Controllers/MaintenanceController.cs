using CarRentalAPIBusinessLayer;
using CarRentalDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/Maintenance")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {

        [HttpGet("All", Name = "GetAllMaintenance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<MaintenanceDTO>> GetAllMaintenance()
        {
            List<MaintenanceDTO> MaintenanceList = clsMaintenance.GetAllMaintenance();

            if (MaintenanceList.Count == 0)
                return NotFound("No Maintenance Found!");

            return Ok(MaintenanceList);
        }


        [HttpGet("{ID}", Name = "GetMaintenanceByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<MaintenanceDTO> GetMaintenanceByID(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }

            var Maintenance = clsMaintenance.Find(ID);

            if (Maintenance == null)
            {
                return NotFound($"Maintenance with ID: {ID} not found.");
            }

            MaintenanceDTO MDTO = Maintenance.MDTO;

            return Ok(MDTO);
        }


        [HttpPost(Name = "AddMaintenance")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<MaintenanceDTO> AddMaintenance(MaintenanceDTO NewMaintenanceDTO)
        {
            if (NewMaintenanceDTO == null)//you will add all validation here 
            {
                return BadRequest("Invalid Maintenance Data.");
            }

            var Maintenance = new clsMaintenance(NewMaintenanceDTO);

            if (!Maintenance.Save())
            {
                return StatusCode(500, new { Message = "Error : Adding Maintenance." });
            }

            NewMaintenanceDTO.MaintenanceID = Maintenance.MaintenanceID;

            return CreatedAtRoute("GetMaintenanceByID", new { ID = NewMaintenanceDTO.MaintenanceID }, NewMaintenanceDTO);
        }


        [HttpPut("{ID}", Name = "UpdateMaintenance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<MaintenanceDTO> UpdateMaintenance(int ID, MaintenanceDTO updatedMaintenance)
        {
            if (ID < 1 || updatedMaintenance == null) //you will add all validation here 
            {
                return BadRequest("Invalid Maintenance data.");
            }

            var Maintenance = clsMaintenance.Find(ID);

            if (Maintenance == null)
            {
                return NotFound($"Maintenance with ID {ID} not found.");
            }

            Maintenance.CarID = updatedMaintenance.CarID;
            Maintenance.StartDate = updatedMaintenance.StartDate;
            Maintenance.EndDate = updatedMaintenance.EndDate;
            Maintenance.MaintenanceType = updatedMaintenance.MaintenanceType;
            Maintenance.ProblemDescription = updatedMaintenance.ProblemDescription;
            Maintenance.Cost = updatedMaintenance.Cost;
            Maintenance.MaintenanceStatus = updatedMaintenance.MaintenanceStatus;

            if (!Maintenance.Save())
            {
                return StatusCode(500, new { Message = "Error : Updating Maintenance." });
            }

            return Ok(Maintenance.MDTO);
        }


        //[HttpDelete("{ID}", Name = "DeleteMaintenance")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public ActionResult DeleteMaintenance(int ID)
        //{
        //    if (ID < 1)
        //    {
        //        return BadRequest($"Not Accepted ID: {ID}");
        //    }


        //    if (clsMaintenance.DeleteMaintenance(ID))
        //    {
        //        return Ok($"Maintenance with ID: {ID} has been Deleted.");
        //    }
        //    else
        //    {
        //        return NotFound($"Maintenance with ID {ID} not found. no rows deleted!");
        //    }

        //}


    }

}
