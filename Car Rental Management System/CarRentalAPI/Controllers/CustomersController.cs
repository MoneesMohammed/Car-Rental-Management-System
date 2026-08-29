using CarRentalAPIBusinessLayer;
using CarRentalDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/Customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {

        [HttpGet("All", Name = "GetAllCustomers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<CustomerDTO>> GetAllCustomers()
        {
            List<CustomerDTO> CustomerList = clsCustomer.GetAllCustomers();

            if (CustomerList.Count == 0)
                return NotFound("No Customers Found!");

            return Ok(CustomerList);
        }


        [HttpGet("{ID}", Name = "GetCustomerByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<CustomerDTO> GetCustomerByID(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }

            var Customer = clsCustomer.Find(ID);

            if (Customer == null)
            {
                return NotFound($"Customer with ID: {ID} not found.");
            }

            CustomerDTO CDTO = Customer.CDTO;

            return Ok(CDTO);
        }


        [HttpPost(Name = "AddCustomer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CustomerDTO> AddCustomer(CustomerDTO NewCustomerDTO)
        {
            if (NewCustomerDTO == null)//you will add all validation here 
            {
                return BadRequest("Invalid Customer Data.");
            }

            var Customer = new clsCustomer(NewCustomerDTO);

            if (!Customer.Save())
            {
                return StatusCode(500, new { Message = "Error : Adding Customer." });
            }

            NewCustomerDTO.CustomerID = Customer.CustomerID;
            NewCustomerDTO.PDTO.PersonID = Customer.PersonID;
            NewCustomerDTO.CreateDate = DateTime.Now;

            return CreatedAtRoute("GetCustomerByID", new { ID = NewCustomerDTO.CustomerID }, NewCustomerDTO);
        }


        [HttpPut("{ID}", Name = "UpdateCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CustomerDTO> UpdateCustomer(int ID, CustomerDTO updatedCustomer)
        {
            if (ID < 1 || updatedCustomer == null) //you will add all validation here 
            {
                return BadRequest("Invalid Customer data.");
            }

            var Customer = clsCustomer.Find(ID);

            if (Customer == null)
            {
                return NotFound($"Customer with ID {ID} not found.");
            }

            
            Customer.DrivingLicenseNo = updatedCustomer.DrivingLicenseNo;
            Customer.DrivingLicenseExpiryDate = updatedCustomer.DrivingLicenseExpiryDate;
            Customer.IsActive = updatedCustomer.IsActive;


            Customer.FirstName   = updatedCustomer.PDTO.FirstName;
            Customer.SecondName  = updatedCustomer.PDTO.SecondName;
            Customer.ThirdName   = updatedCustomer.PDTO.ThirdName;
            Customer.LastName    = updatedCustomer.PDTO.LastName;
            Customer.DateOfBirth = updatedCustomer.PDTO.DateOfBirth;
            Customer.Gender      = updatedCustomer.PDTO.Gender;
            Customer.Address     = updatedCustomer.PDTO.Address;
            Customer.Phone       = updatedCustomer.PDTO.Phone;
            Customer.Email       = updatedCustomer.PDTO.Email;
            Customer.ImagePath   = updatedCustomer.PDTO.ImagePath;



            if (!Customer.Save())
            {
                return StatusCode(500, new { Message = "Error : Updating Customer." });
            }

            return Ok(Customer.CDTO);
        }


        [HttpDelete("{ID}", Name = "DeleteCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteCustomer(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not Accepted ID: {ID}");
            }


            if (clsCustomer.DeleteCustomer(ID))
            {
                return Ok($"Customer with ID: {ID} has been Deleted.");
            }
            else
            {
                return NotFound($"Customer with ID {ID} not found. no rows deleted!");
            }

        }


        [HttpPut("{ID}/Activate", Name = "ActivateCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult ActivateCustomer(int ID)
        {
            if (ID < 1) //you will add all validation here 
            {
                return BadRequest("Invalid Customer data.");
            }

            bool IsActive = clsCustomer.ActivateCustomer(ID);

            if (!IsActive)
            {
                return NotFound($"Customer with ID {ID} not found, or already Active.");
            }

            return Ok($"Customer with ID: {ID} has been Activated.");
        }


        [HttpGet("AllDeactivate", Name = "GetAllDeactivateCustomers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<CustomerDTO>> GetAllDeactivateCustomers()
        {
            List<CustomerDTO> CustomerList = clsCustomer.GetAllDeactivateCustomers();

            if (CustomerList.Count == 0)
                return NotFound("No Customers Found!");

            return Ok(CustomerList);
        }

    }

}
