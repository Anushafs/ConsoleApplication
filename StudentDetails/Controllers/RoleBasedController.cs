using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentDetails.Data;

namespace StudentDetails.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleBasedController : ControllerBase
    {
        private readonly ApplicationDbContext _Dbcontext;
        public RoleBasedController(ApplicationDbContext Dbcontext)
        {
            _Dbcontext = Dbcontext;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult GetAllStudents()
        {
            var students = _Dbcontext.Students.ToList();
            return Ok(students);
        }

    }
}
