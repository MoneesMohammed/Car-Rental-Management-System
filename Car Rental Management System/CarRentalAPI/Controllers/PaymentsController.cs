using CarRentalAPIBusinessLayer;
using CarRentalDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/Payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {

        [HttpGet("All", Name = "GetAllPayments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<PaymentDTO>> GetAllPayments()
        {
            List<PaymentDTO> PaymentList = clsPayment.GetAllPayments();

            if (PaymentList.Count == 0)
                return NotFound("No Payments Found!");

            return Ok(PaymentList);
        }


        [HttpGet("{ID}", Name = "GetPaymentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<PaymentDTO> GetPaymentByID(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }

            var Payment = clsPayment.Find(ID);

            if (Payment == null)
            {
                return NotFound($"Payment with ID: {ID} not found.");
            }

            PaymentDTO PDTO = Payment.PDTO;

            return Ok(PDTO);
        }


        [HttpPost(Name = "AddPayment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PaymentDTO> AddPayment(PaymentDTO NewPaymentDTO)
        {
            if (NewPaymentDTO == null)//you will add all validation here 
            {
                return BadRequest("Invalid Payment Data.");
            }

            var Payment = new clsPayment(NewPaymentDTO);

            if (!Payment.Save())
            {
                return StatusCode(500, new { Message = "Error : Adding Payment." });
            }

            NewPaymentDTO.PaymentID = Payment.PaymentID;

            return CreatedAtRoute("GetPaymentByID", new { ID = NewPaymentDTO.PaymentID }, NewPaymentDTO);
        }


        [HttpPut("{ID}", Name = "UpdatePayment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PaymentDTO> UpdatePayment(int ID, PaymentDTO updatedPayment)
        {
            if (ID < 1 || updatedPayment == null) //you will add all validation here 
            {
                return BadRequest("Invalid Payment data.");
            }

            var Payment = clsPayment.Find(ID);

            if (Payment == null)
            {
                return NotFound($"Payment with ID {ID} not found.");
            }

            Payment.RentalContractID = updatedPayment.RentalContractID;
            Payment.PaymentMethod = updatedPayment.PaymentMethod;
            Payment.PaymentDate = updatedPayment.PaymentDate;
            Payment.AmountPaid = updatedPayment.AmountPaid;
            Payment.PaymentStatus = updatedPayment.PaymentStatus;
            Payment.TransactionReference = updatedPayment.TransactionReference;

            if (!Payment.Save())
            {
                return StatusCode(500, new { Message = "Error : Updating Payment." });
            }

            return Ok(Payment.PDTO);
        }


        //[HttpDelete("{ID}", Name = "DeletePayment")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public ActionResult DeletePayment(int ID)
        //{
        //    if (ID < 1)
        //    {
        //        return BadRequest($"Not Accepted ID: {ID}");
        //    }


        //    if (clsPayment.DeletePayment(ID))
        //    {
        //        return Ok($"Payment with ID: {ID} has been Deleted.");
        //    }
        //    else
        //    {
        //        return NotFound($"Payment with ID {ID} not found. no rows deleted!");
        //    }

        //}


    }

}
