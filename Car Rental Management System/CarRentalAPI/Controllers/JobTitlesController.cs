using CarRentalAPIBusinessLayer;
using CarRentalDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{

    //[Route("api/[controller]")]
    [Route("api/JobTitles")]
    [ApiController]
    public class JobTitlesController : ControllerBase
    {

        [HttpGet("All", Name = "GetAllJobTitles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<JobTitleDTO>> GetAllJobTitles()
        {
            List<JobTitleDTO> JobTitleList = clsJobTitle.GetAllJobTitles();

            if (JobTitleList.Count == 0)
                return NotFound("No JobTitles Found!");

            return Ok(JobTitleList);
        }


        [HttpGet("{ID}", Name = "GetJobTitleByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<JobTitleDTO> GetJobTitleByID(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }

            var JobTitle = clsJobTitle.Find(ID);

            if (JobTitle == null)
            {
                return NotFound($"JobTitle with ID: {ID} not found.");
            }

            JobTitleDTO JDTO = JobTitle.JDTO;
            
            return Ok(JDTO);
        }


    }

}
