using HRManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityChangesController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        [HttpGet]
        [EndpointSummary("Get all Activity Changes")]
        [EndpointDescription("Get all Activity Changes")]
        public async Task<ActionResult> GetHrActivityChanges()
        {
            List<HrActivityChange>? hrActivityChanges = await _context.HrActivityChanges.ToListAsync();
            return hrActivityChanges != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = hrActivityChanges,
                    Message = "Activity Changes fetched successfully"
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = hrActivityChanges,
                    Message = "No Activity Changes found"
                });
        }

        [HttpGet("{id}")]
        [EndpointSummary("Get Activity Change by Id")]
        [EndpointDescription("Get Activity Change by Id")]
        public async Task<ActionResult> GetHrActivityChange(long id)
        {
            HrActivityChange? hrActivityChange = await _context.HrActivityChanges.FindAsync(id);
            return hrActivityChange != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = hrActivityChange,
                    Message = "Activity Change fetched successfully"
                })
                : NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = hrActivityChange,
                    Message = "Activity Change not found"
                });
        }

        [HttpPost]
        [EndpointSummary("Create Activity Change")]
        [EndpointDescription("Create Activity Change")]
        public async Task<ActionResult> CreateHrActivityChange(HrActivityChange hrActivityChange)
        {
            if (hrActivityChange == null)
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = hrActivityChange,
                    Message = "Invalid input data"
                });
            }

            if (await _context.HrActivityChanges.AnyAsync(x => x.Id == hrActivityChange.Id))
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = hrActivityChange,
                    Message = "Activity Change already exists"
                });
            }

            _ = _context.HrActivityChanges.Add(hrActivityChange);
            _ = await _context.SaveChangesAsync();
            return Ok(new DefaultResponseModel()
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = hrActivityChange,
                Message = "Activity Change created successfully"
            });
        }

        [HttpGet("ActivityChangesPagination")]
        [EndpointSummary("Get Activity Changes with Pagination")]
        [EndpointDescription("Get Activity Changes with Pagination")]
        public async Task<ActionResult> GetHrActivityChangesWithPagination(int page, int pagesize)
        {
            if (page < 1 || pagesize < 1)
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Invalid page or pagesize"
                });
            }

            int totalChanges = await _context.HrActivityChanges.CountAsync();
            List<HrActivityChange>? hrActivityChanges = await _context.HrActivityChanges
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();
            return hrActivityChanges != null && hrActivityChanges.Count > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = new
                    {
                        TotalActivityChanges = totalChanges,
                        ActivityChanges = hrActivityChanges
                    },
                    Message = "Activity Changes fetched successfully"
                })
                : NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "No Activity Changes found"
                });
        }

        #region Update Activity Change
        [HttpPut("{id}")]
        [EndpointSummary("Update Activity Change")]
        [EndpointDescription("Update Activity Change")]
        public async Task<ActionResult> UpdateHrActivityChange(long id, HrActivityChange hrActivityChange)
        {
            if (hrActivityChange == null || id != hrActivityChange.Id)
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Invalid input data."
                });
            }

            _context.Entry(hrActivityChange).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new DefaultResponseModel()
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = hrActivityChange,
                Message = "Activity Change updated successfully."
            });
        }
        #endregion

        #region Delete Activity Change
        [HttpDelete("{id}")]
        [EndpointSummary("Delete Activity Change")]
        [EndpointDescription("Delete Activity Change by id")]
        public async Task<ActionResult> DeleteHrActivityChange(long id)
        {
            HrActivityChange? hrActivityChange = await _context.HrActivityChanges.FindAsync(id);
            if (hrActivityChange == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Activity Change not found."
                });
            }

            _context.HrActivityChanges.Remove(hrActivityChange);
            await _context.SaveChangesAsync();
            return Ok(new DefaultResponseModel()
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = hrActivityChange,
                Message = "Activity Change deleted successfully."
            });
        }
        #endregion

    }
}
