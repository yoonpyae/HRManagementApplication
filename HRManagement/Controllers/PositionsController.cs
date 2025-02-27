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


    }
}
