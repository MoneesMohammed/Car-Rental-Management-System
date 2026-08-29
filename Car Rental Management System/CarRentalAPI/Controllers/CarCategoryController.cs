using CarRentalAPIBusinessLayer;
using CarRentalDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/CarCategorys")]
    [ApiController]
    public class CarCategoryController : ControllerBase
    {

        [HttpGet("All", Name = "GetAllCarCategorys")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<CarCategoryDTO>> GetAllCarCategorys()
        {
            List<CarCategoryDTO> CarCategoryList = clsCarCategory.GetAllCarCategory();

            if (CarCategoryList.Count == 0)
                return NotFound("No CarCategory Found!");

            return Ok(CarCategoryList);
        }


        [HttpGet("{ID}", Name = "GetCarCategoryByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CarCategoryDTO> GetCarCategoryByID(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }

            var CarCategory = clsCarCategory.Find(ID);

            if (CarCategory == null)
            {
                return NotFound($"CarCategory with ID: {ID} not found.");
            }

            CarCategoryDTO CDTO = CarCategory.CDTO;

            return Ok(CDTO);
        }




    }

}
