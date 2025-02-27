using HRManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public BranchesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [EndpointSummary("Get all Branches")]
        [EndpointDescription("Get all Branches")]
        public async Task<ActionResult> GetHrBranches()
        {
            List<HrBranch>? hrBranches = await _context.HrBranches.ToListAsync();
            return hrBranches != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = hrBranches,
                    Message = "Branches fetched successfully"
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = hrBranches,
                    Message = "No Branches found"
                });
        }

        [HttpGet("{id}")]
        [EndpointSummary("Get branch by id")]
        [EndpointDescription("Get a branch by id")]
        public async Task<IActionResult> GetBrachById(long id)
        {
            var branch = await _context.HrBranches.FindAsync(id);
            return branch != null
               ? Ok(new DefaultResponseModel()
               {
                   Success = true,
                   StatusCode = StatusCodes.Status200OK,
                   Data = branch,
                   Message = "Successfully fetched."
               })
               : NotFound(new DefaultResponseModel()
               {
                   Success = false,
                   StatusCode = StatusCodes.Status404NotFound,
                   Data = null,
                   Message = "Branch Not Found."
               });
        }
    }
}
