using CarRentalAPIBusinessLayer;
using CarRentalDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{

    //[Route("api/[controller]")]
    [Route("api/FuelTypes")]
    [ApiController]
    public class FuelTypesController : ControllerBase
    {

        [HttpGet("All", Name = "GetAllFuelTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<FuelTypeDTO>> GetAllFuelTypes()
        {
            List<FuelTypeDTO> FuelTypeList = clsFuelType.GetAllFuelTypes();

            if (FuelTypeList.Count == 0)
                return NotFound("No FuelTypes Found!");

            return Ok(FuelTypeList);
        }


        [HttpGet("{ID}", Name = "GetFuelTypeByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<FuelTypeDTO> GetFuelTypeByID(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }

            var FuelType = clsFuelType.Find(ID);

            if (FuelType == null)
            {
                return NotFound($"FuelType with ID: {ID} not found.");
            }

            FuelTypeDTO FDTO = FuelType.FDTO;

            return Ok(FDTO);
        }

    }

}
