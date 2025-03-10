using HRManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        #region Get Employee
        [HttpGet]
        [EndpointSummary("Get all Employees")]
        public async Task<IActionResult> GetEmployee()
        {
            var employee = await _context.ViHrEmployees.ToListAsync();
            return employee != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = employee,
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
        #endregion


        #region Get by id
        [HttpGet("{id}")]
        [EndpointSummary("Get employee by id")]
        public async Task<IActionResult> GetById(string id)
        {
            var employee = await _context.HrEmployees.FindAsync(id);
            return employee != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = employee,
                    Message = "Success"
                }) :
                NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Failed"
                });
        }
        #endregion

        //#region Create
        //[HttpPost]
        //[EndpointSummary("Create Employee")]
        //public async Task<IActionResult> CreateEmployee([FromBody] HrEmployee employee)
        //{
        //    if (await _context.HrEmployees.AnyAsync(x => x.EmployeeId == employee.EmployeeId))
        //    {
        //        return BadRequest(new DefaultResponseModel()
        //        {
        //            Success = false,
        //            StatusCode = StatusCodes.Status400BadRequest,
        //            Data = employee,
        //            Message = "Employee already exists"
        //        });
        //    }

        //    _context.HrEmployees.Add(employee);
        //    int row = await _context.SaveChangesAsync();
        //    return row > 0
        //        ? Ok(new DefaultResponseModel()
        //        {
        //            Success = true,
        //            StatusCode = StatusCodes.Status200OK,
        //            Data = employee,
        //            Message = "Success"
        //        }) :
        //        BadRequest(new DefaultResponseModel()
        //        {
        //            Success = false,
        //            StatusCode = StatusCodes.Status400BadRequest,
        //            Data = employee,
        //            Message = "Failed"
        //        });
        //}
        //#endregion
    }
}
