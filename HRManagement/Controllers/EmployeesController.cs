using HRManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context=context;

    }
}
