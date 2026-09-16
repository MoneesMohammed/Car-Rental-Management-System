using CarRentalAPIBusinessLayer;
using CarRentalDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{

    //[Route("api/[controller]")]
    [Route("api/Reservations")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {

        [HttpGet("All", Name = "GetAllReservations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<ReservationDTO>> GetAllReservations()
        {
            List<ReservationDTO> ReservationList = clsReservation.GetAllReservations();

            if (ReservationList.Count == 0)
                return NotFound("No Reservations Found!");

            return Ok(ReservationList);
        }


        [HttpGet("{ID}", Name = "GetReservationByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<ReservationDTO> GetReservationByID(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }

            var Reservation = clsReservation.Find(ID);

            if (Reservation == null)
            {
                return NotFound($"Reservation with ID: {ID} not found.");
            }

            ReservationDTO RDTO = Reservation.RDTO;

            return Ok(RDTO);
        }


        [HttpPost(Name = "AddReservation")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ReservationDTO> AddReservation(ReservationDTO NewReservationDTO)
        {
            if (NewReservationDTO == null)//you will add all validation here 
            {
                return BadRequest("Invalid Reservation Data.");
            }

            //string message = "";

            //if (!clsCar.CheckCarAvailability(NewReservationDTO.CarID,ref message))
            //{
            //    return BadRequest(message);
            //}

            var Reservation = new clsReservation(NewReservationDTO);

            var result = Reservation.Save();

            switch (result)
            {
                case clsReservation.enSaveResult.Success:
                    NewReservationDTO.ReservationID = Reservation.ReservationID;
                    NewReservationDTO.PickupBranchID = Reservation.PickupBranchID;
                    return CreatedAtRoute("GetReservationByID", new { ID = NewReservationDTO.ReservationID }, NewReservationDTO);

                case clsReservation.enSaveResult.CustomerNotFound:
                    return BadRequest("Customer Not Found.");

                case clsReservation.enSaveResult.CustomerInactive:
                    return BadRequest("Customer Inactive.");

                case clsReservation.enSaveResult.CarNotFound:
                    return BadRequest("Car Not Found.");

                case clsReservation.enSaveResult.CarNotAvailable:
                    return BadRequest("Car Not Available.");

                case clsReservation.enSaveResult.PickupBranchNotFound:
                    return BadRequest("Pickup Branch Not Found.");

                case clsReservation.enSaveResult.ReturnBranchNotFound:
                    return BadRequest("Return Branch Not Found.");

                case clsReservation.enSaveResult.PickupBranchInactive:
                    return BadRequest("Pickup Branch Inactive.");

                case clsReservation.enSaveResult.ReturnBranchInactive:
                    return BadRequest("Return Branch Inactive.");

                case clsReservation.enSaveResult.PickupAndReturnDateTimeInvalid:
                    return BadRequest("Pickup And Return DateTime Invalid.");

                case clsReservation.enSaveResult.PickupDateTimeInvalid:
                    return BadRequest("Pickup DateTime Invalid.");

                case clsReservation.enSaveResult.CarReserved:
                    return BadRequest("Car Reserved.");

                default:
                    return StatusCode(500, new { Message = "Error : Adding Reservation." });
            }


        }


        [HttpPut("{ID}", Name = "UpdateReservation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ReservationDTO> UpdateReservation(int ID, ReservationDTO updatedReservation)
        {
            if (ID < 1 || updatedReservation == null) //you will add all validation here 
            {
                return BadRequest("Invalid Reservation data.");
            }

            var Reservation = clsReservation.Find(ID);

            if (Reservation == null)
            {
                return NotFound($"Reservation with ID {ID} not found.");
            }

            Reservation.CustomerID = updatedReservation.CustomerID;
            Reservation.CarID = updatedReservation.CarID;
            Reservation.PickupBranchID = updatedReservation.PickupBranchID;
            Reservation.ReturnsBranchID = updatedReservation.ReturnsBranchID;
            Reservation.PickupDateTime = updatedReservation.PickupDateTime;
            Reservation.ExpectedReturnDateTime = updatedReservation.ExpectedReturnDateTime;
            Reservation.AgreedPrice = updatedReservation.AgreedPrice;

            var result = Reservation.Save();

            switch (result)
            {
                case clsReservation.enSaveResult.Success:
                    return Ok(Reservation.RDTO);

                case clsReservation.enSaveResult.CustomerNotFound:
                    return BadRequest("Customer Not Found.");

                case clsReservation.enSaveResult.CustomerInactive:
                    return BadRequest("Customer Inactive.");

                case clsReservation.enSaveResult.CarNotFound:
                    return BadRequest("Car Not Found.");

                case clsReservation.enSaveResult.CarNotAvailable:
                    return BadRequest("Car Not Available.");

                case clsReservation.enSaveResult.PickupBranchNotFound:
                    return BadRequest("Pickup Branch Not Found.");

                case clsReservation.enSaveResult.ReturnBranchNotFound:
                    return BadRequest("Return Branch Not Found.");

                case clsReservation.enSaveResult.PickupBranchInactive:
                    return BadRequest("Pickup Branch Inactive.");

                case clsReservation.enSaveResult.ReturnBranchInactive:
                    return BadRequest("Return Branch Inactive.");

                case clsReservation.enSaveResult.PickupAndReturnDateTimeInvalid:
                    return BadRequest("Pickup And Return DateTime Invalid.");

                case clsReservation.enSaveResult.PickupDateTimeInvalid:
                    return BadRequest("Pickup DateTime Invalid.");

                case clsReservation.enSaveResult.CarReserved:
                    return BadRequest("Car Reserved.");

                default:
                    return StatusCode(500, new { Message = "Error : Updating Reservation." });
            }

        }


        [HttpDelete("{ID}", Name = "DeleteReservation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteReservation(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }


            if (clsReservation.DeleteReservation(ID))
            {
                return Ok($"Reservation with ID: {ID} has been Deleted.");
            }
            else
            {
                return NotFound($"Reservation with ID {ID} not found. no rows deleted!");
            }

        }


    }

}
