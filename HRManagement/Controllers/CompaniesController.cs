using HRManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        [EndpointSummary("Get all companies")]
        [EndpointDescription("Get all companies")]
        public async Task<IActionResult> GetCompanies()
        {
            List<ViHrCompany>? companies = await _context.ViHrCompanies.ToListAsync();
            return companies != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Message = "Successfully fetched.",
                    Data = new
                    {
                        TotalCompanies = companies.Count,
                        Companies = companies
                    }
                })
                : NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "No companies found."
                });
        }

        [HttpGet("{id}")]
        [EndpointSummary("Get company by id")]
        [EndpointDescription("Get a company by id")]
        public async Task<IActionResult> GetCompanyById(string id)
        {
            HrCompany? company = await _context.HrCompanies.FindAsync(id);
            return company != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = company,
                    Message = "Successfully fetched."
                })
                : NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Company Not Found."
                });
        }

        [HttpPost]
        [EndpointSummary("Create Company")]
        [EndpointDescription("Create a new company")]
        public async Task<IActionResult> CreateCompany([FromBody] HrCompany company)
        {
            if (await _context.HrCompanies.AnyAsync(x => x.CompanyId == company.CompanyId))
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Company already exists."
                });
            }
            _ = _context.HrCompanies.Add(company);
            int addedRows = await _context.SaveChangesAsync();
            return addedRows > 0
                      ? Ok(new DefaultResponseModel()
                      {
                          Success = true,
                          StatusCode = StatusCodes.Status201Created,
                          Data = company,
                          Message = "Company created successfully."
                      })
                      : BadRequest(new DefaultResponseModel()
                      {
                          Success = false,
                          StatusCode = StatusCodes.Status400BadRequest,
                          Data = null,
                          Message = "Failed to create company."
                      });
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete company")]
        [EndpointDescription("Delete a company")]
        public async Task<IActionResult> DeleteCompany(string id)
        {
            HrCompany? company = await _context.HrCompanies.FindAsync(id);
            if (company != null)
            {
                _ = _context.HrCompanies.Remove(company);
                int deletedRows = await _context.SaveChangesAsync();
                return deletedRows > 0
                    ? Ok(new DefaultResponseModel()
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status200OK,
                        Data = company,
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

        [HttpPut("{id}")]
        [EndpointSummary("Update company")]
        [EndpointDescription("Update a company")]
        public async Task<IActionResult> UpdateCompany(string id, [FromBody] HrCompany company)
        {
            HrCompany? existingCompany = await _context.HrCompanies.FindAsync(id);
            if (existingCompany != null)
            {
                existingCompany.CompanyName = company.CompanyName;
                existingCompany.JoinDate = company.JoinDate;
                existingCompany.LicenseNo = company.LicenseNo;
                existingCompany.ContactPerson = company.ContactPerson;
                existingCompany.PrimaryPhone = company.PrimaryPhone;
                existingCompany.OtherPhone = company.OtherPhone;
                existingCompany.Email = company.Email;
                existingCompany.HouseNo = company.HouseNo;
                existingCompany.StreetId = company.StreetId;
                existingCompany.StreetName = company.StreetName;
                existingCompany.TownshipId = company.TownshipId;
                existingCompany.StateId = company.StateId;
                existingCompany.Photo = company.Photo;
                existingCompany.Status = company.Status;
                existingCompany.CreatedOn = company.CreatedOn;
                existingCompany.CreatedBy = "devAdmin";
                existingCompany.UpdatedOn = DateTime.Now;
                existingCompany.UpdatedBy = "devAdmin";
                existingCompany.DeletedOn = company.DeletedOn;
                existingCompany.DeletedBy = company.DeletedBy;
                existingCompany.Remark = company.Remark;

                _ = _context.HrCompanies.Update(existingCompany);
                int updatedRows = await _context.SaveChangesAsync();

                return updatedRows > 0
                    ? Ok(new DefaultResponseModel()
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status200OK,
                        Data = existingCompany,
                        Message = "Company updated successfully."
                    })
                    : BadRequest(new DefaultResponseModel()
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Data = null,
                        Message = "Failed to update company."
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
