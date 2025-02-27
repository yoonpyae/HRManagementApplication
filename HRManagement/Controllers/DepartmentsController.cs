using HRManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        [EndpointSummary("Get all Departments")]
        [EndpointDescription("Get all Departments")]
        public async Task<ActionResult> GetHrDepartments()
        {
            List<HrDepartment>? hrDepartments = await _context.HrDepartments.ToListAsync();
            return hrDepartments != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = hrDepartments,
                    Message = "Departments fetched successfully"
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = hrDepartments,
                    Message = "No Departments found"
                });
        }
    }
}
