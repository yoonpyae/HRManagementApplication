using HRManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobOpeningController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        [EndpointSummary("Get all Job Opening")]
        public async Task<IActionResult> GetJobOpening()
        {
            List<ViHrJobOpening>? jobOpening = await _context.ViHrJobOpenings.Where(x => !x.DeletedOn.HasValue).ToListAsync();
            return jobOpening != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = jobOpening,
                    Message = "Succuess"
                }) : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = jobOpening,
                    Message = "Failed"
                });
        }


        [HttpGet("{id}")]
        [EndpointSummary("Get Job Opening by Id")]
        public async Task<IActionResult> GetById(long id)
        {
            var jobOpening = await _context.HrJobOpenings.FindAsync(id);
            return jobOpening != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = jobOpening,
                    Message = "Succuess"
                }) : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = jobOpening,
                    Message = "Failed"
                });
        }

    }
}
