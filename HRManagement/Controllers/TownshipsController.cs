using HRManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TownshipsController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        #region Get All Townships
        [HttpGet]
        [EndpointSummary("Get all townships")]
        [EndpointDescription("Get all townships")]
        public async Task<IActionResult> GetTownships()
        {
            List<ViHrTownship>? townships = await _context.ViHrTownships.ToListAsync();
            return townships == null || townships.Count == 0
                ? NotFound(new DefaultResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "No townships found."
                })
                : Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Message = "Townships retrieved successfully.",
                    Data = new
                    {
                        TotalTownships = townships.Count,
                        Townships = townships
                    }
                });
        }
        #endregion

        #region Get Township by ID
        [HttpGet("{id}")]
        [EndpointSummary("Get township by id")]
        [EndpointDescription("Get a township by id")]
        public async Task<IActionResult> GetTownshipById(int id)
        {
            HrTownship? township = await _context.HrTownships.FindAsync(id);
            return township != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = township,
                    Message = "Township retrieved successfully."
                })
                : NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Township not found."
                });
        }
        #endregion

        #region Get Township by Pagination
        [HttpGet("TsPagination")]
        [EndpointSummary("Get Township by Pagination")]
        [EndpointDescription("Get Township by Pagination")]
        public async Task<IActionResult> GetTownshipByPagination(int page, int pageSize)
        {
            if (page < 0 || pageSize <= 0)
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Invalid page or page size."
                });
            }

            List<ViHrTownship>? townships = await _context.ViHrTownships.Skip(page * pageSize).Take(pageSize).ToListAsync();
            return townships == null || !townships.Any()
                ? NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "No townships found."
                })
                : Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Message = "Townships retrieved successfully.",
                    Data = new
                    {
                        TotalTownships = townships.Count,
                        Townships = townships
                    }
                });
        }
        #endregion

        #region Create Township
        [HttpPost]
        [EndpointSummary("Create Township")]
        [EndpointDescription("Create a new township")]
        public async Task<IActionResult> CreateTownship([FromBody] HrTownship township)
        {
            if (township == null || string.IsNullOrEmpty(township.TownshipName))
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Invalid township data."
                });
            }

            if (await _context.HrTownships.AnyAsync(x => x.TownshipName == township.TownshipName))
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = township,
                    Message = "Township already exists."
                });
            }

            _ = _context.HrTownships.Add(township);
            int createTs = await _context.SaveChangesAsync();
            return createTs > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status201Created,
                    Data = township,
                    Message = "Township created successfully."
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = township,
                    Message = "Failed to create township."
                });
        }
        #endregion

        #region Update Township
        [HttpPut("{id}")]
        [EndpointSummary("Update Township")]
        [EndpointDescription("Update a township")]
        public async Task<IActionResult> UpdateTownship(int id, [FromBody] HrTownship township)
        {
            if (township == null)
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Invalid township data."
                });
            }

            HrTownship? townshipData = await _context.HrTownships.FindAsync(id);
            if (townshipData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Township Not Found."
                });
            }
            townshipData.TownshipId = township.TownshipId;
            townshipData.TownshipName = township.TownshipName;
            townshipData.TownshipNameMm = township.TownshipNameMm;
            townshipData.StateId = township.StateId;

            _ = _context.HrTownships.Update(townshipData);
            int UpdateTs = await _context.SaveChangesAsync();
            return UpdateTs > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = townshipData,
                    Message = "Township updated successfully"
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = townshipData,
                    Message = "Township update failed"
                });
        }
        #endregion

        #region Delete Township
        [HttpDelete("{id}")]
        [EndpointSummary("Delete Township")]
        [EndpointDescription("Delete a township")]
        public async Task<IActionResult> DeleteTownship(int id)
        {
            HrTownship? township = await _context.HrTownships.FindAsync(id);
            if (township == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Township not found."
                });
            }
            _ = _context.HrTownships.Remove(township);
            int deleteTs = await _context.SaveChangesAsync();
            return deleteTs > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = township,
                    Message = "Township deleted successfully."
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = township,
                    Message = "Failed to delete township."
                });
        }
        #endregion
    }
}

