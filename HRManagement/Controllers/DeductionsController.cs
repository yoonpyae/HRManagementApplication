using HRManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeductionsController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        [EndpointSummary("Get all Deduction")]
        [EndpointDescription("Get all Deductions")]
        public async Task<IActionResult> GetHrDections()
        {
            List<ViHrDeduction>? deductions = await _context.ViHrDeductions.Where(x => !x.DeletedOn.HasValue).ToListAsync();
            return deductions != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = deductions,
                    Message = "Deductions fetched successfully!"
                }
                ) : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = deductions,
                    Message = "No Deductions found"

                });

        }
    }
}
