using HRManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreetsController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        [EndpointSummary("Get all streets")]
        [EndpointDescription("Get all streets")]
        public async Task<IActionResult> GetStreets()
        {
            List<ViHrStreet>? streets = await _context.ViHrStreets.ToListAsync();
            return streets != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Message = "Successfully fetched.",
                    Data = new
                    {
                        TotalStreets = streets.Count,
                        Streets = streets
                    }
                })
                : NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "No streets found."
                });
        }

        [HttpGet("{id}")]
        [EndpointSummary("Get street by id")]
        [EndpointDescription("Get a street by id")]
        public async Task<IActionResult> GetStreetById(int id)
        {
            HrStreet? street = await _context.HrStreets.FindAsync(id);
            return street != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = street,
                    Message = "Successfully fetched."
                })
                : NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Street Not Found."
                });
        }

        [HttpPost]
        [EndpointSummary("Create Street")]
        [EndpointDescription("Create a new street")]
        public async Task<IActionResult> CreateStreet([FromBody] HrStreet street)
        {
            if (await _context.HrStreets.AnyAsync(x => x.StreetName == street.StreetName))
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Street already exists."
                });
            }

            _ = _context.HrStreets.Add(street);
            int createSt = await _context.SaveChangesAsync();
            return createSt > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = street,
                    Message = "Street created successfully."
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Street creation failed."
                });
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete Street")]
        [EndpointDescription("Delete a street by id")]
        public async Task<IActionResult> DeleteStreet(int id)
        {
            HrStreet? street = await _context.HrStreets.FindAsync(id);
            if (street == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Street Not Found."
                });
            }
            _ = _context.HrStreets.Remove(street);
            int deleteSt = await _context.SaveChangesAsync();
            return deleteSt > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = street,
                    Message = "Street deleted successfully."
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Street deletion failed."
                });
        }

        [HttpPut("{id}")]
        [EndpointSummary("Update Street")]
        [EndpointDescription("Update a street by id")]
        public async Task<IActionResult> UpdateStreet(int id, [FromBody] HrStreet street)
        {
            HrStreet? existingStreet = await _context.HrStreets.FindAsync(id);
            if (existingStreet != null)
            {
                existingStreet.StreetId = street.StreetId;
                existingStreet.StreetName = street.StreetName;
                existingStreet.TownshipId = street.TownshipId;
                existingStreet.Lat = street.Lat;
                existingStreet.Long = street.Long;
                existingStreet.StreetNameMm = street.StreetNameMm;
                _ = _context.HrStreets.Update(existingStreet);
                int updatedSt = await _context.SaveChangesAsync();
                return updatedSt > 0
                    ? Ok(new DefaultResponseModel()
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status200OK,
                        Data = existingStreet,
                        Message = "Street updated successfully."
                    })
                    : BadRequest(new DefaultResponseModel()
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Data = null,
                        Message = "Street update failed."
                    });
            }
            return NotFound(new DefaultResponseModel()
            {
                Success = false,
                StatusCode = StatusCodes.Status404NotFound,
                Data = null,
                Message = "Street Not Found."
            });
        }
    }
}
