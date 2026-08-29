using CarRentalAPIBusinessLayer;
using CarRentalDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{

    //[Route("api/[controller]")]
    [Route("api/Cars")]
    [ApiController]
    public class CarsController : ControllerBase
    {

        [HttpGet("All", Name = "GetAllCars")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<CarDTO>> GetAllCars()
        {
            List<CarDTO> CarList = clsCar.GetAllCars();

            if (CarList.Count == 0)
                return NotFound("No Cars Found!");

            return Ok(CarList);
        }


        [HttpGet("{ID}", Name = "GetCarByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<CarDTO> GetCarByID(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }

            var Car = clsCar.Find(ID);

            if (Car == null)
            {
                return NotFound($"Car with ID: {ID} not found.");
            }

            CarDTO CDTO = Car.CDTO;

            return Ok(CDTO);
        }


        [HttpPost(Name = "AddCar")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CarDTO> AddCar(CarDTO NewCarDTO)
        {
            if (NewCarDTO == null)//you will add all validation here 
            {
                return BadRequest("Invalid Car Data.");
            }

            var Car = new clsCar(NewCarDTO);

            if (!Car.Save())
            {
                return StatusCode(500, new { Message = "Error : Adding Car." });
            }

            NewCarDTO.CarID = Car.CarID;

            return CreatedAtRoute("GetCarByID", new { ID = NewCarDTO.CarID }, NewCarDTO);
        }


        [HttpPut("{ID}", Name = "UpdateCar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CarDTO> UpdateCar(int ID, CarDTO updatedCar)
        {
            if (ID < 1 || updatedCar == null) //you will add all validation here 
            {
                return BadRequest("Invalid Car data.");
            }

            var Car = clsCar.Find(ID);

            if (Car == null)
            {
                return NotFound($"Car with ID {ID} not found.");
            }

            Car.Make = updatedCar.Make;
            Car.Model = updatedCar.Model;
            Car.Year = updatedCar.Year;
            Car.PlateNumber = updatedCar.PlateNumber;
            Car.VIN = updatedCar.VIN;
            Car.Color = updatedCar.Color;
            Car.NumberOfSeats = updatedCar.NumberOfSeats;
            Car.TransmissionType = updatedCar.TransmissionType;
            Car.FuelTypeID = updatedCar.FuelTypeID;
            Car.CarCategoryID = updatedCar.CarCategoryID;
            Car.CarStatus = updatedCar.CarStatus;
            Car.DailyRentalPrice = updatedCar.DailyRentalPrice;
            Car.CurrentMileage = updatedCar.CurrentMileage;
            Car.CurrentBranchID = updatedCar.CurrentBranchID;

            if (!Car.Save())
            {
                return StatusCode(500, new { Message = "Error : Updating Car." });
            }

            return Ok(Car.CDTO);
        }


        [HttpDelete("{ID}", Name = "DeleteCar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteCar(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }


            if (clsCar.DeleteCar(ID))
            {
                return Ok($"Car with ID: {ID} has been Deleted.");
            }
            else
            {
                return NotFound($"Car with ID {ID} not found. no rows deleted!");
            }

        }


    }

}
