using CarRentalAPIBusinessLayer;
using CarRentalDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{

    //[Route("api/[controller]")]
    [Route("api/RentalContracts")]
    [ApiController]
    public class RentalContractsController : ControllerBase
    {

        [HttpGet("All", Name = "GetAllRentalContracts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<RentalContractDTO>> GetAllRentalContracts()
        {
            List<RentalContractDTO> RentalContractList = clsRentalContract.GetAllRentalContracts();

            if (RentalContractList.Count == 0)
                return NotFound("No Rental Contracts Found!");

            return Ok(RentalContractList);
        }


        [HttpGet("{ID}", Name = "GetRentalContractByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<RentalContractDTO> GetRentalContractByID(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }

            var RentalContract = clsRentalContract.Find(ID);

            if (RentalContract == null)
            {
                return NotFound($"Rental Contract with ID: {ID} not found.");
            }

            RentalContractDTO RDTO = RentalContract.RCDTO;

            return Ok(RDTO);
        }


        [HttpPost(Name = "AddRentalContract")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<RentalContractDTO> AddRentalContract(RentalContractDTO NewRContractDTO)
        {
            if (NewRContractDTO == null )//you will add all validation here 
            {
                return BadRequest("Invalid Rental Contract Data.");
            }

            if (NewRContractDTO.FuelLevelUponReceipt > 100 || NewRContractDTO.FuelLevelUponReceipt < 0 ) 
            {
                return BadRequest("Mast be Fuel Level , 0 between 100");
            }

            if (NewRContractDTO.OdometerAtPickup < 0)
            {
                return BadRequest("Mast be Odometer At Pickup , Greater than 0");
            }

            var RentalContract = new clsRentalContract(NewRContractDTO);

            if (!RentalContract.Save())
            {
                return StatusCode(500, new { Message = "Error : Adding Rental Contract." });
            }

            NewRContractDTO.RentalContractID = RentalContract.RentalContractID;

            return CreatedAtRoute("GetRentalContractByID", new { ID = NewRContractDTO.RentalContractID }, NewRContractDTO);
        }


        [HttpPut("{ID}", Name = "UpdateRentalContract")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<RentalContractDTO> UpdateRentalContract(int ID, RentalContractDTO updatedRentalContract)
        {
            if (ID < 1 || updatedRentalContract == null) //you will add all validation here 
            {
                return BadRequest("Invalid Rental Contract data.");
            }

            var RentalContract = clsRentalContract.Find(ID);

            if (RentalContract == null)
            {
                return NotFound($"RentalContract with ID {ID} not found.");
            }

            RentalContract.ContractNumber = updatedRentalContract.ContractNumber;
            RentalContract.ReservationID = updatedRentalContract.ReservationID;
            RentalContract.ActualPickupDateTime = updatedRentalContract.ActualPickupDateTime;
            RentalContract.ActualReturnDateTime = updatedRentalContract.ActualReturnDateTime;
            RentalContract.OdometerAtPickup = updatedRentalContract.OdometerAtPickup;
            RentalContract.OdometerAtReturn = updatedRentalContract.OdometerAtReturn;
            RentalContract.FuelLevelUponReceipt = updatedRentalContract.FuelLevelUponReceipt;
            RentalContract.FuelLevelUponReturn = updatedRentalContract.FuelLevelUponReturn;
            RentalContract.BasicPrice = updatedRentalContract.BasicPrice;
            RentalContract.DiscountAmount = updatedRentalContract.DiscountAmount;
            RentalContract.AdditionalFeesAmount = updatedRentalContract.AdditionalFeesAmount;
            RentalContract.TotalAmount = updatedRentalContract.TotalAmount;
            RentalContract.ContractStatus = updatedRentalContract.ContractStatus;

            if (!RentalContract.Save())
            {
                return StatusCode(500, new { Message = "Error : Updating RentalContract." });
            }

            return Ok(RentalContract.RCDTO);
        }


        [HttpDelete("{ID}", Name = "DeleteRentalContract")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteRentalContract(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }


            if (clsRentalContract.DeleteRentalContract(ID))
            {
                return Ok($"Rental Contract with ID: {ID} has been Deleted.");
            }
            else
            {
                return NotFound($"Rental Contract with ID {ID} not found. no rows deleted!");
            }

        }


    }

}
