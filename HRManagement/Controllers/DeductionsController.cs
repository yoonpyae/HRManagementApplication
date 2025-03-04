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

        [HttpGet("{id}")]
        [EndpointSummary("Get Deduction by Id")]
        public async Task<IActionResult> GetHrDeductions(long id)
        {
            var deductions = await _context.HrDeductions.FindAsync(id);
            return deductions!=null
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

        [HttpGet("by")]
        [EndpointSummary("Get Deduction by CompanyId, BranchId, DeptId")]
        public async Task<IActionResult> GetByCompanyAsync(string companyid, long? branchid, long? depId)
        {
            IReadOnlyList<ViHrDeduction>? deduction = new List<ViHrDeduction>();

         if (!string.IsNullOrEmpty(companyid) && branchid.HasValue && depId.HasValue)
            {
                deduction = await _context.ViHrDeductions.Where(x =>
                !x.DeletedOn.HasValue && x.CompanyId == companyid && x.BranchId == branchid && x.DeptId == depId).ToListAsync();
            }
            else if (!string.IsNullOrEmpty(companyid) && branchid.HasValue)
            {
                deduction = await _context.ViHrDeductions.Where(x =>
                !x.DeletedOn.HasValue && x.CompanyId == companyid && x.BranchId == branchid).ToListAsync();
            }
            else if (!string.IsNullOrEmpty(companyid))
            {
                deduction = await _context.ViHrDeductions.Where(x =>
                !x.DeletedOn.HasValue && x.CompanyId == companyid).ToListAsync();
            }

            return Ok(new DefaultResponseModel()
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = deduction
            });
        }

        [HttpGet("by-deptId")]
        [EndpointSummary("Get Deduction by DepartmentId")]
        public async Task<ActionResult<ViHrDeduction?>> GetByDeptId(long? deptId)
        {
            var deductions = await _context.ViHrDeductions
                .Where(x => x.DeptId == deptId && !x.DeletedOn.HasValue)
                .ToListAsync();

            return Ok(new DefaultResponseModel()
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = deductions,
                Message="Deductions fetched successfully!"
            });
        }



    }
}
