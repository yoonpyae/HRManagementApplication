using HRManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        [EndpointSummary("Get all Positions")]
        [EndpointDescription("Get all Positions")]
        public async Task<ActionResult> GetHrPositions()
        {
            List<HrPosition>? hrPositions = await _context.HrPositions.ToListAsync();
            return hrPositions != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = hrPositions,
                    Message = "Positions fetched successfully"
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = hrPositions,
                    Message = "No Positions found"
                });
        }

        [HttpGet("{id}")]
        [EndpointSummary("Get Position by Id")]
        [EndpointDescription("Get Position by Id")]
        public async Task<ActionResult> GetHrPosition(long id)
        {
            HrPosition? hrPosition = await _context.HrPositions.FindAsync(id);
            return hrPosition != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = hrPosition,
                    Message = "Position fetched successfully"
                })
                : NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = hrPosition,
                    Message = "Position not found"
                });
        }

        [HttpPost]
        [EndpointSummary("Create Position")]
        [EndpointDescription("Create Position")]
        public async Task<ActionResult> CreateHrPosition([FromBody] HrPosition hrPosition)
        {
            if (hrPosition == null)
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Position creation failed"
                });
            }

            if (await _context.HrPositions.AnyAsync(x => x.PositionName == hrPosition.PositionName))
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = hrPosition,
                    Message = "Position already exists"
                });
            }

            _ = await _context.HrPositions.AddAsync(hrPosition);
            int createPosition = await _context.SaveChangesAsync();
            return createPosition > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status201Created,
                    Data = hrPosition,
                    Message = "Position created successfully"
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = hrPosition,
                    Message = "Position creation failed"
                });
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete Position by Id")]
        [EndpointDescription("Delete Position by Id")]
        public async Task<ActionResult> DeleteHrPosition(long id)
        {
            HrPosition? hrPosition = await _context.HrPositions.FindAsync(id);
            if (hrPosition == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Position not found"
                });
            }
            hrPosition.DeletedOn = DateTime.Now;
            hrPosition.DeletedBy = "devAdmin";
            _ = _context.HrPositions.Remove(hrPosition);
            int deletePosition = await _context.SaveChangesAsync();
            return deletePosition > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = hrPosition,
                    Message = "Position deleted successfully"
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = hrPosition,
                    Message = "Position deletion failed"
                });
        }

        [HttpPut("{id}")]
        [EndpointSummary("Update Position")]
        [EndpointDescription("Update Position")]
        public async Task<ActionResult> UpdateHrPosition(long id, [FromBody] HrPosition hrPosition)
        {
            if (hrPosition == null)
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Position update failed"
                });
            }
            HrPosition? existingPosition = await _context.HrPositions.FindAsync(id);
            if (existingPosition == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Position not found"
                });
            }

            existingPosition.DeptId = hrPosition.DeptId;
            existingPosition.PositionName = hrPosition.PositionName;
            existingPosition.Status = hrPosition.Status;
            existingPosition.UpdatedOn = DateTime.Now;
            existingPosition.UpdatedBy = "devAdmin";
            _ = _context.HrPositions.Update(existingPosition);
            int updatePosition = await _context.SaveChangesAsync();
            return updatePosition > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = existingPosition,
                    Message = "Position updated successfully"
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = existingPosition,
                    Message = "Position update failed"
                });
        }


    }
}
