using HRManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        [EndpointSummary("Get all Branches")]
        [EndpointDescription("Get all Branches")]
        public async Task<ActionResult> GetHrBranches()
        {
            List<ViHrBranch>? hrBranches = await _context.ViHrBranches.ToListAsync();
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

        [HttpGet("by-companyId")]
        [EndpointSummary("Get Branches by CompanyId")]
        [EndpointDescription("Get Branches by CompanyId")]
        public async Task<ActionResult<ViHrBranch>> GetByDeptId(string companyId)
        {
            return Ok(new DefaultResponseModel()
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _context.ViHrBranches.Where(x => x.CompanyId == companyId).ToListAsync(),
                Message = "Branches fetched successfully"
            });
        }

        [HttpGet("{id}")]
        [EndpointSummary("Get branch by id")]
        [EndpointDescription("Get a branch by id")]
        public async Task<IActionResult> GetBrachById(long id)
        {
            ViHrBranch? branch = await _context.ViHrBranches.FindAsync(id);
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

        #region create branch
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
#endregion

        #region Update Branch
        [HttpPut("{id}")]
        [EndpointSummary("Update Branch")]
        [EndpointDescription("Update a branch by id")]
        public async Task<IActionResult> UpdateBranch(long id, [FromBody] HrBranch branch)
        {
            if (branch == null)
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Branch is required."
                });
            }
            HrBranch? branchData = await _context.HrBranches.FindAsync(id);
            if (branchData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Branch Not Found."
                });
            }

            branchData.BranchName = branch.BranchName;
            branchData.CompanyId = branch.CompanyId;
            branchData.BranchName = branch.BranchName;
            branchData.ContactPerson = branch.ContactPerson;
            branchData.PrimaryPhone = branch.PrimaryPhone;
            branchData.OtherPhone = branch.OtherPhone;
            branchData.Email = branch.Email;
            branchData.HouseNo = branch.HouseNo;
            branchData.StreetId = branch.StreetId;
            branchData.StreetName = branch.StreetName;
            branchData.TownshipId = branch.TownshipId;
            branchData.StateId = branch.StateId;
            branchData.Photo = branch.Photo;
            branchData.IsDefault = branch.IsDefault;
            branchData.IsAutoDeduction = branch.IsAutoDeduction;
            branchData.Status = branch.Status;
            branchData.UpdatedOn = DateTime.Now;
            branchData.UpdatedBy = "devAdmin";

            _ = _context.HrBranches.Update(branchData);
            int updatedRows = await _context.SaveChangesAsync();
            return updatedRows > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = branchData,
                    Message = "Branch updated successfully."
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Branch update failed."
                });
        }
        #endregion

        #region Delete Branch
        [HttpDelete("{id}")]
        [EndpointSummary("Delete Branch")]
        [EndpointDescription("Delete a branch by id")]
        public async Task<IActionResult> DeleteBranch(long id)
        {
            HrBranch? branch = await _context.HrBranches.FindAsync(id);
            if (branch == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Branch Not Found."
                });
            }
            _ = _context.HrBranches.Remove(branch);
            int deletedRows = await _context.SaveChangesAsync();
            return deletedRows > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = null,
                    Message = "Branch deleted successfully."
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Branch deletion failed."
                });
        }
        #endregion
    }
}
