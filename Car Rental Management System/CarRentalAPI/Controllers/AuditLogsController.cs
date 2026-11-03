using CarRentalAPIBusinessLayer;
using CarRentalDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{

    //[Route("api/[controller]")]

    [Authorize]
    [Route("api/AuditLogs")]
    [ApiController]
    public class AuditLogsController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet("All", Name = "GetAllAuditLogs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<AuditLogDTO>> GetAllAuditLogs()
        {
            List<AuditLogDTO> AuditLogList = clsAuditLog.GetAllAuditLogs();

            if (AuditLogList.Count == 0)
                return NotFound("No AuditLogs Found!");

            return Ok(AuditLogList);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{ID}", Name = "GetAuditLogByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<AuditLogDTO> GetAuditLogByID(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }

            var AuditLog = clsAuditLog.Find(ID);

            if (AuditLog == null)
            {
                return NotFound($"AuditLog with ID: {ID} not found.");
            }

            AuditLogDTO ADTO = AuditLog.ALDTO;

            return Ok(ADTO);
        }

    }

}
