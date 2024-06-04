using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentDetails.Data;
using StudentDetails.Models.Entity;

namespace StudentDetails.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    
    public class StudentAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _Dbcontext;
        public StudentAPIController(ApplicationDbContext Dbcontext)
        {
            _Dbcontext = Dbcontext;
        }

        [Route("api/[controller]")]
        [HttpGet]
        public async Task<IActionResult> GetId(Guid Id)
        {
            var student = await _Dbcontext.Students.FindAsync(Id);
            if (student == null)
                return NotFound();

            return Ok(student);
        }


        [Route("api/List")]
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var user = await _Dbcontext.Students.ToListAsync();
              return Ok(user);
        }

        [Route("api/[controller]")]
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Post([FromBody] Student st)
        {
            if (st == null)
            {
                return BadRequest();
            }
            var student = _Dbcontext.Students.AddAsync(st);
            
             await _Dbcontext.SaveChangesAsync();
            return Ok(new
            {
                Message = " Student data added"
                
            });
        }

        [Route("api/[controller]")]
        [HttpPut]
        //[Route("{Id}")]
        public async Task<IActionResult> Put(Guid Id, [FromBody] Student st)
        {
            var existingstudent = await _Dbcontext.Students.FindAsync(Id);
            if (existingstudent == null)
            {
                return NotFound();
            }
            existingstudent.Name = st.Name;
            existingstudent.Email = st.Email;
            existingstudent.Phone = st.Phone;
            existingstudent.Subscribed = st.Subscribed;

            _Dbcontext.Entry(existingstudent).State = EntityState.Modified;
           await  _Dbcontext.SaveChangesAsync();

            return Ok(new
            {
                message = "Data Updated Successfully!!!",

            });
        }


        [Route("api/[controller]")]
        [HttpDelete]
        //[Route("{id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var existingstudent = await _Dbcontext.Students.FindAsync(Id);
            if (existingstudent == null)
            {
                return NotFound();
            }
            _Dbcontext.Students.Remove(existingstudent);
            await _Dbcontext.SaveChangesAsync();

            return Ok(new
            {
                message = "data deleted successfully!!!",

            });
        }

    }

}
