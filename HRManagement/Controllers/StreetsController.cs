﻿using HRManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreetsController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        #region Get All Streets
        [HttpGet]
        [EndpointSummary("Get all streets")]
        [EndpointDescription("Retrieve all available streets.")]
        public async Task<IActionResult> GetStreets()
        {
            List<ViHrStreet>? streets = await _context.ViHrStreets.ToListAsync();
            return streets != null && streets.Any()
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Message = "List of streets retrieved successfully.",
                    Data = new { TotalStreets = streets.Count, Streets = streets }
                })
                : NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "No streets found."
                });
        }
        #endregion

        #region Get Street By ID
        [HttpGet("{id}")]
        [EndpointSummary("Get street by ID")]
        [EndpointDescription("Retrieve a street using its ID.")]
        public async Task<IActionResult> GetStreetById(int id)
        {
            HrStreet? street = await _context.HrStreets.FindAsync(id);
            return street != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = street,
                    Message = "Street details retrieved successfully."
                })
                : NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Street not found."
                });
        }
        #endregion

        #region Get Streets by Pagination
        [HttpGet("StreetPagination")]
        [EndpointSummary("Get streets by pagination")]
        [EndpointDescription("Retrieve a paginated list of streets.")]
        public async Task<IActionResult> GetStreetByPagination(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Page number and page size must be greater than zero."
                });
            }

            List<HrStreet> streets = await _context.HrStreets.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return streets.Any()
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Message = "Streets retrieved successfully.",
                    Data = new { TotalStreets = streets.Count, Streets = streets }
                })
                : NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "No streets found for the specified page."
                });
        }
        #endregion

        #region Create Street
        [HttpPost]
        [EndpointSummary("Create a new street")]
        [EndpointDescription("Add a new street to the database.")]
        public async Task<IActionResult> CreateStreet([FromBody] HrStreet street)
        {
            if (street == null || string.IsNullOrEmpty(street.StreetName))
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Street name is required."
                });
            }

            if (await _context.HrStreets.AnyAsync(x => x.StreetName == street.StreetName))
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "A street with this name already exists."
                });
            }

            _ = _context.HrStreets.Add(street);
            int createResult = await _context.SaveChangesAsync();
            return createResult > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status201Created,
                    Data = street,
                    Message = "Street created successfully."
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Failed to create street."
                });
        }
        #endregion

        #region Update Street
        [HttpPut("{id}")]
        [EndpointSummary("Update Street")]
        [EndpointDescription("Update a street by id")]
        public async Task<IActionResult> UpdateStreet(int id, [FromBody] HrStreet street)
        {
            if (street == null)
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Street data is required."
                });
            }

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
        #endregion

        #region Delete Street
        [HttpDelete("{id}")]
        [EndpointSummary("Delete a street")]
        [EndpointDescription("Remove a street from the database.")]
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
                    Message = "Street not found."
                });
            }

            _ = _context.HrStreets.Remove(street);
            int deleteResult = await _context.SaveChangesAsync();
            return deleteResult > 0
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
                    Message = "Failed to delete street."
                });
        }
        #endregion
    }
}
