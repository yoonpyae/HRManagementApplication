using HRManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeBanksController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        [EndpointSummary("Get all employee banks")]
        [EndpointDescription("Get all employee banks")]
        public async Task<IActionResult> GetEmployeeBanks()
        {
            List<HrEmployeeBank>? employeeBanks = await _context.HrEmployeeBanks.ToListAsync();
            return employeeBanks != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Message = "Successfully fetched.",
                    Data = new
                    {
                        TotalEmployeeBanks = employeeBanks.Count,
                        EmployeeBanks = employeeBanks
                    }
                })
                : NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "No employee banks found."
                });
        }

        [HttpGet("{employeeId}/{bankId}")]
        [EndpointSummary("Get employee bank by id")]
        [EndpointDescription("Get an employee bank by id")]
        public async Task<IActionResult> GetEmployeeBankById(string employeeId, string bankId)
        {
            HrEmployeeBank? employeeBank = await _context.HrEmployeeBanks.FindAsync(employeeId, bankId);
            return employeeBank != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = employeeBank,
                    Message = "Successfully fetched."
                })
                : NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Employee Bank Not Found."
                });
        }

        [HttpPost]
        [EndpointSummary("Create Employee Bank")]
        [EndpointDescription("Create an employee bank")]
        public async Task<IActionResult> CreateEmployeeBank([FromBody] HrEmployeeBank employeeBank)
        {
            if (await _context.HrEmployeeBanks.AnyAsync(x => x.EmployeeId == employeeBank.EmployeeId))
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Employee Bank already exists."
                });
            }

            _ = _context.HrEmployeeBanks.Add(employeeBank);
            int addedRows = await _context.SaveChangesAsync();
            return addedRows > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status201Created,
                    Data = employeeBank,
                    Message = "Employee Bank created successfully."
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Failed to create Employee Bank."
                });

        }

        [HttpDelete("{employeeId}/{bankId}")]
        [EndpointSummary("Delete Employee Bank")]
        [EndpointDescription("Delete an employee bank by id")]
        public async Task<IActionResult> DeleteEmployeeBank(string employeeId, string bankId)
        {
            HrEmployeeBank? employeeBank = await _context.HrEmployeeBanks.FindAsync(employeeId, bankId);
            if (employeeBank == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Employee Bank Not Found."
                });
            }
            _ = _context.HrEmployeeBanks.Remove(employeeBank);
            int deleteTs = await _context.SaveChangesAsync();
            return deleteTs > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = employeeBank,
                    Message = "Employee Bank deleted successfully."
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Failed to delete Employee Bank."
                });
        }

        [HttpPut("{employeeId}/{bankId}")]
        [EndpointSummary("Update Employee Bank")]
        [EndpointDescription("Update an employee bank by id")]
        public async Task<IActionResult> UpdateEmployeeBank(string employeeId, string bankId, [FromBody] HrEmployeeBank updatedEmployeeBank)
        {
            HrEmployeeBank? employeeBank = await _context.HrEmployeeBanks.FindAsync(employeeId, bankId);
            if (employeeBank == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Employee Bank Not Found."
                });
            }
            employeeBank.BankAccNo = updatedEmployeeBank.BankAccNo;
            employeeBank.BankName = updatedEmployeeBank.BankName;
            employeeBank.Status = updatedEmployeeBank.Status;
            employeeBank.UpdatedOn = DateTime.Now;
            employeeBank.UpdatedBy = "devadmin";
            _context.HrEmployeeBanks.Update(employeeBank);
            int updateTs = await _context.SaveChangesAsync();
            return updateTs > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = employeeBank,
                    Message = "Employee Bank updated successfully."
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Failed to update Employee Bank."
                });
        }
    }
}