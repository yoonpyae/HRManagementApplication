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
            HrJobOpening? jobOpening = await _context.HrJobOpenings.FindAsync(id);
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

        [HttpPost]
        [EndpointSummary("Create new Job Opening")]
        public async Task<IActionResult> CreateJobOpen(HrJobOpening jobOpening)
        {
            if (await _context.HrJobOpenings.AnyAsync(x => x.Id == jobOpening.Id))
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Failed"
                });
            }

            _ = _context.HrJobOpenings.Add(jobOpening);
            int row = await _context.SaveChangesAsync();
            return row > 0
                          ? Ok(new DefaultResponseModel()
                          {
                              Success = true,
                              StatusCode = StatusCodes.Status200OK,
                              Data = jobOpening,
                              Message = "Success"
                          })
                          : BadRequest(new DefaultResponseModel()
                          {
                              Success = false,
                              StatusCode = StatusCodes.Status400BadRequest,
                              Data = null,
                              Message = "Failed"
                          });
        }

        [HttpPut("{id}")]
        [EndpointSummary("Update Job Opening")]
        public async Task<IActionResult> UpdateJobOpen(long id, [FromBody] HrJobOpening hrJobOpening)
        {
            HrJobOpening? jobOpening = await _context.HrJobOpenings.FindAsync(id);
            if (jobOpening == null)
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Failed"
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
            jobOpening.UpdatedOn = DateTime.Now;
            jobOpening.UpdatedBy = "devAdmin";

            _ = _context.HrJobOpenings.Update(jobOpening);
            int row = await _context.SaveChangesAsync();
            return row > 0
                          ? Ok(new DefaultResponseModel()
                          {
                              Success = true,
                              StatusCode = StatusCodes.Status200OK,
                              Data = jobOpening,
                              Message = "Success"
                          })
                          : BadRequest(new DefaultResponseModel()
                          {
                              Success = false,
                              StatusCode = StatusCodes.Status400BadRequest,
                              Data = null,
                              Message = "Failed"
                          });

        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete Job Opening")]
        public async Task<IActionResult> DeleteJobOpening(long id)
        {
            var jobOpening = await _context.HrJobOpenings.FindAsync(id);
            if (jobOpening != null)
            {
                _context.HrJobOpenings.Remove(jobOpening);
                int row = await _context.SaveChangesAsync();
                return row > 0
                              ? Ok(new DefaultResponseModel()
                              {
                                  Success = true,
                                  StatusCode = StatusCodes.Status200OK,
                                  Data = jobOpening,
                                  Message = "Success"
                              })
                              : BadRequest(new DefaultResponseModel()
                              {
                                  Success = false,
                                  StatusCode = StatusCodes.Status400BadRequest,
                                  Data = null,
                                  Message = "Failed"
                              });
            }
            return NotFound(new DefaultResponseModel()
            {
                Success = false,
                StatusCode = StatusCodes.Status404NotFound,
                Data=null,
                Message="Not Found"
            });
        }
    }
}
