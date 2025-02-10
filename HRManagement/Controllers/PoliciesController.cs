using HRManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoliciesController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        [EndpointSummary("Get all policies")]
        [EndpointDescription("Get all policies")]
        public async Task<IActionResult> GetPolicies()
        {
            List<HrPolicy>? policies = await _context.HrPolicies.ToListAsync();
            return policies != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Message = "Successfully fetched.",
                    Data = new
                    {
                        TotalPolicies = policies.Count,
                        Policies = policies
                    }
                })
                : NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "No policies found."
                });
        }

        [HttpGet("{id}")]
        [EndpointSummary("Get policy by id")]
        [EndpointDescription("Get a policy by id")]
        public async Task<IActionResult> GetPolicyById(long id)
        {
            HrPolicy? policy = await _context.HrPolicies.FindAsync(id);
            return policy != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = policy,
                    Message = "Successfully fetched."
                })
                : NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Policy Not Found."
                });

        }

        [HttpPost]
        [EndpointSummary("Create Policy")]
        [EndpointDescription("Create a new policy")]
        public async Task<IActionResult> CreatePolicy([FromBody] HrPolicy policy)
        {

            if (await _context.HrPolicies.AnyAsync(x => x.Id == policy.Id))
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Policy already exists."
                });
            }
            _ = _context.HrPolicies.Add(policy);
            int CreateP = await _context.SaveChangesAsync();
            return CreateP > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = policy,
                    Message = "Policy created successfully."
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Failed to create policy."
                });
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete Policy")]
        [EndpointDescription("Delete a policy by id")]
        public async Task<IActionResult> DeletePolicy(long id)
        {
            HrPolicy? policy = await _context.HrPolicies.FindAsync(id);
            if (policy == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Policy Not Found."
                });
            }
            _ = _context.HrPolicies.Remove(policy);
            int deleteP = await _context.SaveChangesAsync();
            return deleteP > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = policy,
                    Message = "Policy deleted successfully."
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Failed to delete policy."
                });
        }

        [HttpPut("{id}")]
        [EndpointSummary("Update Policy")]
        [EndpointDescription("Update a policy by id")]
        public async Task<IActionResult> UpdatePolicy(long id, [FromBody] HrPolicy policy)
        {
            HrPolicy? existingPolicy = await _context.HrPolicies.FindAsync(id);
            if (existingPolicy != null)
            {
                existingPolicy.Title = policy.Title;
                existingPolicy.Description = policy.Description;
                existingPolicy.PolicyType = policy.PolicyType;
                existingPolicy.CompanyId = policy.CompanyId;
                existingPolicy.CreatedBy = "DevAdmin";
                existingPolicy.CreatedOn = policy.CreatedOn;
                existingPolicy.UpdatedBy = "DevAdmin";
                existingPolicy.UpdatedOn = policy.UpdatedOn;
                existingPolicy.DeletedBy = "DevAdmin";
                existingPolicy.DeletedOn = policy.DeletedOn;
                existingPolicy.Remark = policy.Remark;
                _ = _context.HrPolicies.Update(existingPolicy);
                int updatedRows = await _context.SaveChangesAsync();
                return updatedRows > 0
                    ? Ok(new DefaultResponseModel()
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status200OK,
                        Data = existingPolicy,
                        Message = "Successfully Updated"
                    })
                    : (IActionResult)StatusCode(StatusCodes.Status500InternalServerError, new DefaultResponseModel()
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Data = null,
                        Message = "Failed to update policy"
                    });
            }
            return NotFound(new DefaultResponseModel()
            {
                Success = false,
                StatusCode = StatusCodes.Status404NotFound,
                Data = null,
                Message = "Policy not found"
            });
        }
    }
}
