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

        [HttpPost]
        [EndpointSummary("Add Allowance")]
        [EndpointDescription("Add Allowance")]
        public async Task<IActionResult> AddAllowance([FromBody] HrAllowance hrAllowance)
        {
            if (hrAllowance == null)
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Allowance is required"
                });
            }
            try
            {
                if (await _context.HrAllowances.AnyAsync(x => x.AllowanceName == hrAllowance.AllowanceName))
                {
                    return BadRequest(new DefaultResponseModel()
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Data = null,
                        Message = "Allowance already exists"
                    });
                }
                _ = _context.HrAllowances.Add(hrAllowance);
                int addedRows = await _context.SaveChangesAsync();
                return addedRows > 0
                    ? Ok(new DefaultResponseModel()
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status200OK,
                        Data = hrAllowance,
                        Message = "Allowance added successfully"
                    })
                    : (IActionResult)StatusCode(StatusCodes.Status500InternalServerError, new DefaultResponseModel()
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Data = null,
                        Message = "Failed to add allowance"
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Data = null,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpPut("{id}")]
        [EndpointSummary("Update Allowance")]
        [EndpointDescription("Update Allowance")]
        public async Task<IActionResult> UpdateAllownce(long id, [FromBody] HrAllowance hrAllowance)
        {
            if(hrAllowance == null)
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Allowance data is required"
                });
            }

            HrAllowance? existingAllowance = await _context.HrAllowances.FindAsync(id);
            if (existingAllowance != null)
            {
                existingAllowance.AllowanceName = hrAllowance.AllowanceName;
                existingAllowance.CompanyId = hrAllowance.CompanyId;
                existingAllowance.BranchId = hrAllowance.BranchId;
                existingAllowance.DeptId = hrAllowance.DeptId;
                existingAllowance.PositionId = hrAllowance.PositionId;
                existingAllowance.Description = hrAllowance.Description;
                existingAllowance.Status = hrAllowance.Status;
                existingAllowance.CreatedOn = hrAllowance.CreatedOn;
                existingAllowance.CreatedBy = hrAllowance.CreatedBy;
                existingAllowance.UpdatedOn = DateTime.Now;
                existingAllowance.UpdatedBy = "DevAdmin";
                existingAllowance.DeletedOn = hrAllowance.DeletedOn;
                existingAllowance.DeletedBy = hrAllowance.DeletedBy;
                existingAllowance.Remark = hrAllowance.Remark;

                _ = _context.HrAllowances.Update(existingAllowance);
                int updatedRows = await _context.SaveChangesAsync();

                return updatedRows > 0
                    ? Ok(new DefaultResponseModel()
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status200OK,
                        Data = existingAllowance,
                        Message = "Successfully Updated"
                    })
                    : (IActionResult)StatusCode(StatusCodes.Status500InternalServerError, new DefaultResponseModel()
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Data = null,
                        Message = "Failed to update allowance"
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

        [HttpDelete("{id}")]
        [EndpointSummary("Delete Allowance")]
        [EndpointDescription("Delete Allowance")]
        public async Task<IActionResult> DeleteAlloance(long id)
        {
            HrAllowance? allowance = await _context.HrAllowances.FindAsync(id);
            if (allowance != null)
            {
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

    }
}
