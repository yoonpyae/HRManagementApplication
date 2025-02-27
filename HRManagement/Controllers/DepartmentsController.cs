﻿using HRManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        [EndpointSummary("Get all Departments")]
        [EndpointDescription("Get all Departments")]
        public async Task<ActionResult> GetHrDepartments()
        {
            List<HrDepartment>? hrDepartments = await _context.HrDepartments.ToListAsync();
            return hrDepartments != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = hrDepartments,
                    Message = "Departments fetched successfully"
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = hrDepartments,
                    Message = "No Departments found"
                });
        }

        [HttpGet("{id}")]
        [EndpointSummary("Get Department by Id")]
        [EndpointDescription("Get Department by Id")]
        public async Task<ActionResult> GetHrDepartment(long id)
        {
            HrDepartment? hrDepartment = await _context.HrDepartments.FindAsync(id);
            return hrDepartment != null
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = hrDepartment,
                    Message = "Department fetched successfully"
                })
                : NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = hrDepartment,
                    Message = "Department not found"
                });
        }

        [HttpPost]
        [EndpointSummary("Create Department")]
        [EndpointDescription("Create Department")]
        public async Task<IActionResult> CreateDepartment([FromBody] HrDepartment department)
        {
            if (department == null)
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Department is required."
                });
            }
            if (await _context.HrDepartments.AnyAsync(x => x.DeptId == department.DeptId))
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Department already exists."
                });
            }

            _ = await _context.HrDepartments.AddAsync(department);
            int CreateDept = await _context.SaveChangesAsync();
            return CreateDept > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status201Created,
                    Data = department,
                    Message = "Department created successfully."
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Department not created."
                });
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete Department")]
        [EndpointDescription("Delete a department by id")]
        public async Task<IActionResult> DeleteDepartment(long id)
        {
            HrDepartment? department = await _context.HrDepartments.FindAsync(id);
            if (department == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "State Not Found."
                });
            }

            _ = _context.HrDepartments.Remove(department);
            int deleteDept = await _context.SaveChangesAsync();
            return deleteDept > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = department,
                    Message = "Department deleted successfully."
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Failed to delete department."
                });
        }

    }
}
