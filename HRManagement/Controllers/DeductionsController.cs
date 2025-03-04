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
            HrDeduction? deductions = await _context.HrDeductions.FindAsync(id);
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

        [HttpGet("by")]
        [EndpointSummary("Get Deduction by CompanyId, BranchId, DeptId")]
        public async Task<IActionResult> GetByCompanyAsync(string companyid, long? branchid, long? depId)
        {
            IReadOnlyList<ViHrDeduction>? deduction = [];

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
            List<ViHrDeduction> deductions = await _context.ViHrDeductions
                .Where(x => x.DeptId == deptId && !x.DeletedOn.HasValue)
                .ToListAsync();

            return Ok(new DefaultResponseModel()
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = deductions,
                Message = "Deductions fetched successfully!"
            });
        }

        //[HttpPost]
        //[EndpointSummary("Add deduction")]
        //public async Task<IActionResult> AddDeduction([FromBody] HrDeduction hrDeduction)
        //{
        //    //if (await _context.HrDeductions.AnyAsync(x => x.DeductionId == hrDeduction.DeductionId))
        //    //{
        //    //    return BadRequest(new DefaultResponseModel()
        //    //    {
        //    //        Success = false,
        //    //        StatusCode = StatusCodes.Status400BadRequest,
        //    //        Data = null,
        //    //        Message = "Desuction not found"
        //    //    });
        //    //}

        //    //_context.HrDeductions.Add(hrDeduction);
        //    //await _context.SaveChangesAsync();
        //    return Ok(new DefaultResponseModel()
        //    {
        //        Success = true,
        //        StatusCode = StatusCodes.Status200OK,
        //        Data = null,
        //        Message = "Deductions fetched successfully!"
        //    });
        //}

        [HttpPost]
        [EndpointSummary("Create deduction")]
        [EndpointDescription("Create a new deduction")]
        public async Task<IActionResult> Creatededuction([FromBody] HrDeduction deduction)
        {
            if (await _context.HrDeductions.AnyAsync(x => x.DeductionId == deduction.DeductionId))
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "deduction already exists."
                });
            }
            _ = _context.HrDeductions.Add(deduction);
            int addedRows = await _context.SaveChangesAsync();
            return addedRows > 0
                      ? Ok(new DefaultResponseModel()
                      {
                          Success = true,
                          StatusCode = StatusCodes.Status201Created,
                          Data = deduction,
                          Message = "deduction created successfully."
                      })
                      : BadRequest(new DefaultResponseModel()
                      {
                          Success = false,
                          StatusCode = StatusCodes.Status400BadRequest,
                          Data = null,
                          Message = "Failed to create deduction."
                      });
        }




        //[HttpPost]
        //[EndpointSummary("Create a new Deduction")]
        //[EndpointDescription("Add a new Deduction to the system")]

        //public async Task<IActionResult> CreateDeduction([FromBody] HrDeduction deduction)
        //{
        //    if (deduction == null)
        //    {
        //        return BadRequest(new DefaultResponseModel()
        //        {
        //            Success = false,
        //            StatusCode = StatusCodes.Status400BadRequest,
        //            Data = null,
        //            Message = "Invalid deduction data."
        //        });
        //    }

        //    _context.HrDeductions.Add(deduction);
        //    int createdRows = await _context.SaveChangesAsync();

        //    return createdRows > 0
        //        ? CreatedAtAction(nameof(GetHrDeductions), new { id = deduction.DeductionId }, new DefaultResponseModel()
        //        {
        //            Success = true,
        //            StatusCode = StatusCodes.Status201Created,
        //            Data = deduction,
        //            Message = "Deduction created successfully!"
        //        })
        //        : BadRequest(new DefaultResponseModel()
        //        {
        //            Success = false,
        //            StatusCode = StatusCodes.Status400BadRequest,
        //            Data = null,
        //            Message = "Failed to create deduction."
        //        });
        //}


        [HttpDelete("id")]
        [EndpointSummary("Delete Deductions")]
        public async Task<IActionResult> DeleteDeduction(long? id)
        {
            var  Deduction = await _context.HrDeductions.FindAsync(id);
            if (Deduction != null)
            {
                _ = _context.HrDeductions.Remove(Deduction);
                int deletedRows = await _context.SaveChangesAsync();
                return deletedRows > 0
                    ? Ok(new DefaultResponseModel()
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status200OK,
                        Data = Deduction,
                        Message = "Company deleted successfully."
                    })
                    : BadRequest(new DefaultResponseModel()
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Data = null,
                        Message = "Failed to delete company."
                    });
            }
            return NotFound(new DefaultResponseModel()
            {
                Success = false,
                StatusCode = StatusCodes.Status404NotFound,
                Data = null,
                Message = "Company Not Found."
            });
        }
    }
}
