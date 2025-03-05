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

        #region Get All Job Openings
        [HttpGet]
        [EndpointSummary("Retrieve all active job openings.")]
        public async Task<IActionResult> GetJobOpening()
        {
            List<ViHrJobOpening>? jobOpenings = await _context.ViHrJobOpenings.Where(x => !x.DeletedOn.HasValue).ToListAsync();
            return jobOpenings.Any()
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = jobOpenings,
                    Message = "Job openings retrieved successfully."
                })
                : NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "No job openings found."
                });
        }
        #endregion

        #region Get Job Opening by ID
        [HttpGet("{id}")]
        [EndpointSummary("Retrieve a specific job opening by ID.")]
        public async Task<IActionResult> GetById(long id)
        {
            HrJobOpening? jobOpening = await _context.HrJobOpenings.FindAsync(id);
            return jobOpening != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = jobOpening,
                    Message = "Job opening retrieved successfully."
                })
                : NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Job opening not found."
                });
        }
        #endregion

        #region Create Job Opening
        [HttpPost]
        [EndpointSummary("Create a new job opening.")]
        public async Task<IActionResult> CreateJobOpen(HrJobOpening jobOpening)
        {
            if (await _context.HrJobOpenings.AnyAsync(x => x.Id == jobOpening.Id))
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "A job opening with this ID already exists."
                });
            }

            _context.HrJobOpenings.Add(jobOpening);
            int row = await _context.SaveChangesAsync();
            return row > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status201Created,
                    Data = jobOpening,
                    Message = "Job opening created successfully."
                })
                : StatusCode(StatusCodes.Status500InternalServerError, new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Data = null,
                    Message = "An error occurred while creating the job opening."
                });
        }
        #endregion

        #region Update Job Opening
        [HttpPut("{id}")]
        [EndpointSummary("Update an existing job opening.")]
        public async Task<IActionResult> UpdateJobOpen(long id, [FromBody] HrJobOpening hrJobOpening)
        {
            HrJobOpening? jobOpening = await _context.HrJobOpenings.FindAsync(id);
            if (jobOpening == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Job opening not found."
                });
            }

            jobOpening.Title = hrJobOpening.Title;
            jobOpening.Description = hrJobOpening.Description;
            jobOpening.NoOfApplicants = hrJobOpening.NoOfApplicants;
            jobOpening.StartOn = hrJobOpening.StartOn;
            jobOpening.EndOn = hrJobOpening.EndOn;
            jobOpening.CompanyId = hrJobOpening.CompanyId;
            jobOpening.BranchId = hrJobOpening.BranchId;
            jobOpening.DeptId = hrJobOpening.DeptId;
            jobOpening.PositionId = hrJobOpening.PositionId;
            jobOpening.OpeningStatus = hrJobOpening.OpeningStatus;
            jobOpening.UpdatedOn = DateTime.UtcNow;
            jobOpening.UpdatedBy = "devAdmin";

            _context.HrJobOpenings.Update(jobOpening);
            int row = await _context.SaveChangesAsync();
            return row > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = jobOpening,
                    Message = "Job opening updated successfully."
                })
                : StatusCode(StatusCodes.Status500InternalServerError, new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Data = null,
                    Message = "An error occurred while updating the job opening."
                });
        }
        #endregion

        #region Delete Job Opening
        [HttpDelete("{id}")]
        [EndpointSummary("Delete a job opening by ID.")]
        public async Task<IActionResult> DeleteJobOpening(long id)
        {
            HrJobOpening? jobOpening = await _context.HrJobOpenings.FindAsync(id);
            if (jobOpening == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Job opening not found."
                });
            }

            _context.HrJobOpenings.Remove(jobOpening);
            int row = await _context.SaveChangesAsync();
            return row > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = jobOpening,
                    Message = "Job opening deleted successfully."
                })
                : StatusCode(StatusCodes.Status500InternalServerError, new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Data = null,
                    Message = "An error occurred while deleting the job opening."
                });
        }
        #endregion
    }
}