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

            string message = "";

            if (!clsCar.CheckCarAvailability(NewReservationDTO.CarID,ref message))
            {
                return BadRequest(message);
            }


            var Reservation = new clsReservation(NewReservationDTO);

            if (!Reservation.Save())
            {
                return StatusCode(500, new { Message = "Error : Adding Reservation." });
            }

            NewReservationDTO.ReservationID = Reservation.ReservationID;

            return CreatedAtRoute("GetReservationByID", new { ID = NewReservationDTO.ReservationID }, NewReservationDTO);
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
            Reservation.BookingStatus = updatedReservation.BookingStatus;

            if (!Reservation.Save())
            {
                return StatusCode(500, new { Message = "Error : Updating Reservation." });
            }

            return Ok(Reservation.RDTO);
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
