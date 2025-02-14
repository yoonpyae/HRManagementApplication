using HRManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatesController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        [EndpointSummary("Get all states")]
        [EndpointDescription("Get all states")]
        public async Task<IActionResult> GetStates()
        {
            List<HrState>? states = await _context.HrStates.ToListAsync();
            return states != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Message = "Successfully fetched.",
                    Data = new
                    {
                        TotalStates = states.Count,
                        States = states
                    }
                })
                : NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "No states found."
                });
        }

        [HttpGet("{id}")]
        [EndpointSummary("Get state by id")]
        [EndpointDescription("Get a state by id")]
        public async Task<IActionResult> GetStateById(int id)
        {
            HrState? state = await _context.HrStates.FindAsync(id);
            return state != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = state,
                    Message = "Successfully fetched."
                })
                : NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "State Not Found."
                });
        }

        [HttpGet("StatePagination")]
        [EndpointSummary("Get State by Pagination")]
        [EndpointDescription("Get State by Pagination")]
        public async Task<IActionResult> GetStateByPagination(int page, int pageSize)
        {
            if (page < 0 || pageSize <= 0)
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Invalid page or pageSize."
                });
            }

            int totalStates = await _context.HrStates.CountAsync();
            List<HrState>? states = await _context.HrStates
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return states != null && states.Count > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Message = "Successfully fetched.",
                    Data = new
                    {
                        TotalStates = totalStates,
                        States = states
                    }
                })
                : NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "No states found."
                });
        }

        [HttpPost]
        [EndpointSummary("Create State")]
        [EndpointDescription("Create a new state")]
        public async Task<IActionResult> CreateState([FromBody] HrState state)
        {
            if (state == null || string.IsNullOrEmpty(state.StateName))
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "State name is required."
                });
            }

            if (await _context.HrStates.AnyAsync(x => x.StateName == state.StateName))
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "State already exists."
                });

            }

            _ = await _context.HrStates.AddAsync(state);
            int CreateSt = await _context.SaveChangesAsync();
            return CreateSt > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status201Created,
                    Data = state,
                    Message = "State created successfully."
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "State creation failed."
                });
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete State")]
        [EndpointDescription("Delete a state by id")]
        public async Task<IActionResult> DeleteState(int id)
        {
            HrState? state = await _context.HrStates.FindAsync(id);
            if (state == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "State Not Found."
                });
            }

            _ = _context.HrStates.Remove(state);
            int deleteState = await _context.SaveChangesAsync();
            return deleteState > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = state,
                    Message = "State deleted successfully."
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Failed to delete state."
                });

        }

        [HttpPut("{id}")]
        [EndpointSummary("Update State")]
        [EndpointDescription("Update a state by id")]
        public async Task<IActionResult> UpdateState(int id, [FromBody] HrState state)
        {
            if (state == null)
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "State data is required."
                });
            }

            HrState? stateData = await _context.HrStates.FindAsync(id);
            if (stateData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "State Not Found."
                });
            }
            stateData.StateName = state.StateName;
            stateData.StateNameMm = state.StateNameMm;
            _ = _context.HrStates.Update(stateData);
            int updateState = await _context.SaveChangesAsync();
            return updateState > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = stateData,
                    Message = "State updated successfully."
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Failed to update state."
                });
        }


    }
}
