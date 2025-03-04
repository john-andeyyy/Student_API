    using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student_API.Data;
using Student_API.Models.DTOs.Address;
using Student_API.Models.Entities;
using System.Diagnostics.Metrics;

namespace Student_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public AddressController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("GetAllAddress")]
        public IActionResult GetAllAddress()
        {
            var AddressList = dbContext.Address
                .Select(a => new
                {
                    a.Id,
                    a.Barangay,
                    a.Province,
                    a.Municipality,
                    a.Country
                })
                .ToList();

            if (!AddressList.Any()) return NotFound("No Address saved in database");

            return Ok(AddressList);
        }


        [HttpGet("GetAddresByID/{id}")]
        public IActionResult GetAddress(int id)
        {
            var address = dbContext.Address.Find(id);
            if (address == null) return NotFound($"Not Found the id: {id}");
            return Ok(address);
        }

        [HttpPost("NewAddress")]
        public IActionResult CreateAddress([FromBody] CreateAddressDTO address)
        {

            bool addressExists = dbContext.Address
            .Where(a => a.Barangay == address.Barangay)
            .Where(a => a.Province == address.Province)
            .Where(a => a.Municipality == address.Municipality)
            .Where(a => a.Country == address.Country)
            .Any();

            if (addressExists) return BadRequest("An address with the same details already exists.");

            
            var newAddd = new Address()
            {
                Barangay = address.Barangay,
                Province = address.Province,
                Municipality = address.Municipality,
                Country = address.Country
            };

            dbContext.Address.Add(newAddd);
            dbContext.SaveChanges();
            return Ok("New Address Added!");
        }

        [HttpPut("Update/{id}")]
        public IActionResult UpdateAddress([FromBody] UpdateAddressDTO updateAddressDTO, int id)
        {
            var addExisting = dbContext.Address.Find(id);

            if (addExisting == null) return NotFound("not found");

            bool addressExists = dbContext.Address
             .Where(a => a.Barangay == updateAddressDTO.Barangay)
             .Where(a => a.Province == updateAddressDTO.Province)
             .Where(a => a.Municipality == updateAddressDTO.Municipality)
             .Where(a => a.Country == updateAddressDTO.Country)
             .Where(a => a.Id != id) 
             .Any();

            if (addressExists) return BadRequest("An address with the same details already exists.");

            addExisting.Barangay = updateAddressDTO.Barangay;
            addExisting.Province = updateAddressDTO.Province;
            addExisting.Municipality = updateAddressDTO.Municipality;
            addExisting.Country = updateAddressDTO.Country;

            dbContext.SaveChanges();

            return Ok(addExisting);
        }


        [HttpDelete("Address/{id}")]
        public IActionResult DeleteAddress(int id)
        {
            var address = dbContext.Address.Find(id);
            if (address == null) return NotFound("Address Not Found!!");

            try
            {
                dbContext.Remove(address);
                dbContext.SaveChanges();
                return Ok("Delete Successfully");
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                if (ex.InnerException is MySqlConnector.MySqlException mysqlEx && mysqlEx.Number == 1451)
                {
                    return BadRequest("Cannot delete address because it is referenced by students.");
                }
                return StatusCode(500, "An error occurred while deleting the address.");
            }
        }


    }
}
