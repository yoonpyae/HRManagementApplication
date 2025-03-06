using HRManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllowanceController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        #region Get All Allowances
        [HttpGet]
        [EndpointSummary("Get all Allowances")]
        [EndpointDescription("Get all Allowances")]
        public async Task<ActionResult> GetHrAllowances()
        {
            List<ViHrAllowance>? hrAllowances = await _context.ViHrAllowances.Where(x => !x.DeletedOn.HasValue).ToListAsync();

            return hrAllowances != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = hrAllowances,
                    Message = "Allowances fetched successfully"
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = hrAllowances,
                    Message = "No Allowances found"
                });
        }
        #endregion

        #region Get Allowance by Id
        [HttpGet("{id}")]
        [EndpointSummary("Get Allowance by Id")]
        [EndpointDescription("Get Allowance by Id")]
        public async Task<ActionResult> GetHrAllowance(long id)
        {
            HrAllowance? hrAllowance = await _context.HrAllowances.FindAsync(id);
            return hrAllowance != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = hrAllowance,
                    Message = "Allowance fetched successfully"
                })
                : NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = hrAllowance,
                    Message = "Allowance not found"
                });
        }
        #endregion

        #region Get Allowances with Pagination
        [HttpGet("AllowancesPagination")]
        [EndpointSummary("Get Allowances with pagination")]
        [EndpointDescription("Get Allowances with pagination")]
        public async Task<ActionResult> GetHrAllowancesWithPagination(int page, int size)
        {
            if (page < 0 || size < 0)
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Invalid page or size"
                });
            }

            List<ViHrAllowance>? hrAllowances = await _context.ViHrAllowances.Where(x => !x.DeletedOn.HasValue)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
            return hrAllowances != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = hrAllowances,
                    Message = "Allowances fetched successfully"
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = hrAllowances,
                    Message = "No Allowances found"
                });
        }
        #endregion

        #region Add Allowance
        [HttpPost]
        [EndpointSummary("Add Allowance")]
        [EndpointDescription("Add Allowance")]
        public async Task<IActionResult> AddAllowance([FromBody] HrAllowance hrAllowance)
        {
            if (await _context.HrAllowances.AnyAsync(x => x.AllowanceId == hrAllowance.AllowanceId))
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Allowance not found"
                });
            }

            _ = _context.HrAllowances.Add(hrAllowance);
            _ = await _context.SaveChangesAsync();

            return Created("api/HrAllowance", new DefaultResponseModel()
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = hrAllowance,
                Message = "Created successfully"
            });
        }
        #endregion

        #region Update Allowance
        [HttpPut("{id}")]
        [EndpointSummary("Update Allowance")]
        [EndpointDescription("Update Allowance")]
        public async Task<IActionResult> UpdateAllowance(long id, [FromBody] HrAllowance allowance)
        {
            if (allowance == null)
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Invalid allowance data."
                });
            }

            HrAllowance? allowanceData = await _context.HrAllowances.FindAsync(id);
            if (allowanceData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Allowance not found"
                });
            }

            allowanceData.AllowanceId = allowance.AllowanceId;
            allowanceData.CompanyId = allowance.CompanyId;
            allowanceData.BranchId = allowance.BranchId;
            allowanceData.DeptId = allowance.DeptId;
            allowanceData.PositionId = allowance.PositionId;
            allowanceData.AllowanceName = allowance.AllowanceName;
            allowanceData.Description = allowance.Description;
            allowanceData.Status = allowance.Status;
            allowanceData.UpdatedOn = DateTime.Now;
            allowanceData.UpdatedBy = "Admin";

            _ = _context.HrAllowances.Update(allowanceData);
            int updatedRows = await _context.SaveChangesAsync();
            return updatedRows > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = allowanceData,
                    Message = "Allowance updated successfully"
                })
                : (IActionResult)StatusCode(StatusCodes.Status500InternalServerError, new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Data = null,
                    Message = "Failed to update allowance"
                });
        }
        #endregion

        #region Delete Allowance
        [HttpDelete("{id}")]
        [EndpointSummary("Delete Allowance")]
        [EndpointDescription("Delete Allowance")]
        public async Task<IActionResult> DeleteAllowance(long id)
        {
            HrAllowance? allowance = await _context.HrAllowances.FindAsync(id);
            if (allowance != null)
            {
                allowance.DeletedOn = DateTime.Now;
                allowance.DeletedBy = "devAdmin";
                _ = _context.HrAllowances.Remove(allowance);
                int deletedRows = await _context.SaveChangesAsync();
                return deletedRows > 0
                    ? Ok(new DefaultResponseModel()
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status200OK,
                        Data = allowance,
                        Message = "Allowance deleted successfully"
                    })
                    : (IActionResult)StatusCode(StatusCodes.Status500InternalServerError, new DefaultResponseModel()
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Data = null,
                        Message = "Failed to delete allowance"
                    });
            }
            return NotFound(new DefaultResponseModel()
            {
                Success = false,
                StatusCode = StatusCodes.Status404NotFound,
                Data = null,
                Message = "Allowance not found"
            });
        }
        #endregion
    }
}
