using CarRentalAPIBusinessLayer;
using CarRentalDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    //[Route("api/[controller]")]
    [Authorize]
    [Route("api/Maintenance")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        [Authorize(Roles = "Admin,Manager")]
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

        [Authorize(Roles = "Admin,Manager")]
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

        [Authorize(Roles = "Admin,Manager")]
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

            var result = Maintenance.Save();

            switch (result)
            {
                case clsMaintenance.enSaveResult.Success:
                    NewMaintenanceDTO.MaintenanceID = Maintenance.MaintenanceID;
                    return CreatedAtRoute("GetMaintenanceByID", new { ID = NewMaintenanceDTO.MaintenanceID }, NewMaintenanceDTO);

                case clsMaintenance.enSaveResult.CarNotFound:
                    return BadRequest("Car Not Found.");

                case clsMaintenance.enSaveResult.CarStatusInvalid:
                    return BadRequest("Car Status Invalid.");

                case clsMaintenance.enSaveResult.MaintenanceTypeInvalid:
                    return BadRequest("Maintenance Type Invalid.");

                case clsMaintenance.enSaveResult.StartDateInvalid:
                    return BadRequest("Start Date Invalid.");

                case clsMaintenance.enSaveResult.CostInvalid:
                    return BadRequest("Cost Invalid.");

                case clsMaintenance.enSaveResult.MaintenanceAlreadyInProgress:
                    return BadRequest("Maintenance Already InProgress.");

                case clsMaintenance.enSaveResult.MaintenanceStatusIsCompleted:
                    return BadRequest("Maintenance Status Is Completed.");

                default:
                    return StatusCode(500, new { Message = "Error : Adding Maintenance." });


            }


        }

        [Authorize(Roles = "Admin,Manager")]
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

            Maintenance.MaintenanceType = updatedMaintenance.MaintenanceType;
            Maintenance.ProblemDescription = updatedMaintenance.ProblemDescription;
            Maintenance.Cost = updatedMaintenance.Cost;
           

            var result = Maintenance.Save();

            switch (result)
            {
                case clsMaintenance.enSaveResult.Success:
                    return Ok(Maintenance.MDTO);

                case clsMaintenance.enSaveResult.CarNotFound:
                    return BadRequest("Car Not Found.");

                case clsMaintenance.enSaveResult.CarStatusInvalid:
                    return BadRequest("Car Status Invalid.");

                case clsMaintenance.enSaveResult.MaintenanceTypeInvalid:
                    return BadRequest("Maintenance Type Invalid.");

                case clsMaintenance.enSaveResult.StartDateInvalid:
                    return BadRequest("Start Date Invalid.");

                case clsMaintenance.enSaveResult.CostInvalid:
                    return BadRequest("Cost Invalid.");

                case clsMaintenance.enSaveResult.MaintenanceAlreadyInProgress:
                    return BadRequest("Maintenance Already InProgress.");

                case clsMaintenance.enSaveResult.MaintenanceStatusIsCompleted:
                    return BadRequest("Maintenance Status Is Completed.");

                default:
                    return StatusCode(500, new { Message = "Error : Updating Maintenance." });


            }

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

        [Authorize(Roles = "Admin,Manager")]
        [HttpPut("{ID}/Complete", Name = "MaintenanceComplete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult CompleteMaintenance(int ID)
        {
            if (ID < 1) //you will add all validation here 
            {
                return BadRequest($"Not Accepted ID: {ID}.");
            }

            bool IsCompleted = clsMaintenance.MaintenanceComplete(ID);

            if (!IsCompleted)
            {
                return NotFound($"Maintenance with ID {ID} not found , or already Completed.");
            }

            return Ok($"Maintenance with ID: {ID} has been Completed.");
        }

    }

}
