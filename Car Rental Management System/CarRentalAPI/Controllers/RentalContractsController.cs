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


            var RentalContract = new clsRentalContract(NewRContractDTO);

            var result = RentalContract.Save();

            switch (result)
            {
                case clsRentalContract.enSaveResult.Success:
                    RentalContract = clsRentalContract.Find(RentalContract.RentalContractID);
                    NewRContractDTO = RentalContract!.RCDTO;

                    return CreatedAtRoute("GetRentalContractByID", new { ID = NewRContractDTO.RentalContractID }, NewRContractDTO);

                case clsRentalContract.enSaveResult.ReservationNotFound:
                    return BadRequest("Reservation Not Found.");

                case clsRentalContract.enSaveResult.ReservationStatusInvalid:
                    return BadRequest("Reservation Status Invalid.");

                case clsRentalContract.enSaveResult.ReservationAlreadyUsed:
                    return BadRequest("Reservation Already Used.");

                case clsRentalContract.enSaveResult.CarNotAvailable:
                    return BadRequest("Car Not Available.");

                case clsRentalContract.enSaveResult.ActualPickupDateTimeInvalid:
                    return BadRequest("Actual Pickup DateTime Invalid.");

                case clsRentalContract.enSaveResult.OdometerAtPickupInvalid:
                    return BadRequest("Odometer At Pickup Invalid.");

                case clsRentalContract.enSaveResult.FuelLevelUponReceiptInvalid:
                    return BadRequest("Fuel Level Upon Receipt Invalid. (Mast be, 0 between 100)");

                case clsRentalContract.enSaveResult.DiscountAmountInvalid:
                    return BadRequest("Discount Amount Invalid.");

                case clsRentalContract.enSaveResult.AdditionalFeesAmountInvalid:
                    return BadRequest("Additional Fees Amount Invalid.");

                case clsRentalContract.enSaveResult.ContractNumberAlreadyUsed:
                    return BadRequest("Contract Number Already Used.");

                default:
                    return StatusCode(500, new { Message = "Error : Adding Rental Contract." });
            }

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
            
            RentalContract.ActualPickupDateTime = updatedRentalContract.ActualPickupDateTime;
            RentalContract.ActualReturnDateTime = updatedRentalContract.ActualReturnDateTime;
            RentalContract.OdometerAtPickup = updatedRentalContract.OdometerAtPickup;
            RentalContract.OdometerAtReturn = updatedRentalContract.OdometerAtReturn;
            RentalContract.FuelLevelUponReceipt = updatedRentalContract.FuelLevelUponReceipt;
            RentalContract.FuelLevelUponReturn = updatedRentalContract.FuelLevelUponReturn;
            
            RentalContract.DiscountAmount = updatedRentalContract.DiscountAmount;
            RentalContract.AdditionalFeesAmount = updatedRentalContract.AdditionalFeesAmount;
            

            var result = RentalContract.Save();

            switch (result)
            {
                case clsRentalContract.enSaveResult.Success:
                    return Ok(RentalContract.RCDTO);

                case clsRentalContract.enSaveResult.ReservationNotFound:
                    return BadRequest("Reservation Not Found.");

                case clsRentalContract.enSaveResult.ReservationStatusInvalid:
                    return BadRequest("Reservation Status Invalid.");

                case clsRentalContract.enSaveResult.ReservationAlreadyUsed:
                    return BadRequest("Reservation Already Used.");

                case clsRentalContract.enSaveResult.CarNotAvailable:
                    return BadRequest("Car Not Available.");

                case clsRentalContract.enSaveResult.ActualPickupDateTimeInvalid:
                    return BadRequest("Actual Pickup DateTime Invalid.");

                case clsRentalContract.enSaveResult.OdometerAtPickupInvalid:
                    return BadRequest("Odometer At Pickup Invalid.");

                case clsRentalContract.enSaveResult.FuelLevelUponReceiptInvalid:
                    return BadRequest("Fuel Level Upon Receipt Invalid. (Mast be, 0 between 100)");

                case clsRentalContract.enSaveResult.DiscountAmountInvalid:
                    return BadRequest("Discount Amount Invalid.");

                case clsRentalContract.enSaveResult.AdditionalFeesAmountInvalid:
                    return BadRequest("Additional Fees Amount Invalid.");

                case clsRentalContract.enSaveResult.ContractNumberAlreadyUsed:
                    return BadRequest("Contract Number Already Used.");

                default:
                    return StatusCode(500, new { Message = "Error : Updating RentalContract." });
            }


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


        [HttpGet("{ID}/PaymentSummary", Name = "GetPaymentSummaryByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PaymentSummaryDTO> GetPaymentSummaryByID(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }

            var PSDTO = clsRentalContract.GetPaymentSummaryByID(ID);

            if (PSDTO == null)
            {
                return NotFound($"Rental Contract with ID: {ID} not found.");
            }

            return Ok(PSDTO);
        }


    }

}
