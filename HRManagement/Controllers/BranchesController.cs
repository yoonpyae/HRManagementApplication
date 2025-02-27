using HRManagement.Models;
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
            HrBranch? branch = await _context.HrBranches.FindAsync(id);
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

        [HttpPost]
        [EndpointSummary("Create Branch")]
        [EndpointDescription("Create Branch")]
        public async Task<ActionResult> CreateHrBranch([FromBody] HrBranch hrBranch)
        {
            if (hrBranch == null)
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Branch is required."
                });
            }

            if (await _context.HrBranches.AnyAsync(x => x.BranchId == hrBranch.BranchId))
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Branch already exists."
                });
            }

            _ = await _context.HrBranches.AddAsync(hrBranch);
            int CreateBranch = await _context.SaveChangesAsync();
            return CreateBranch > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status201Created,
                    Data = hrBranch,
                    Message = "Branch created successfully."
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Branch creation failed."
                });
        }


    }
}
