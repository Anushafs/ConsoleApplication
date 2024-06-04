using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentDetails.Data;
using StudentDetails.Models;
using StudentDetails.Models.Entity;
using StudentDetails.ViewModel;

namespace StudentDetails.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly ApplicationDbContext _Dbcontext;
        public EmployeeController(ApplicationDbContext Dbcontext)
        {
            _Dbcontext = Dbcontext;
        }
        [Authorize]
        [HttpGet]
        [Route("GetData")]
        public string GetData()
        {
            return "Authenticated with JWT";
        }

        [HttpGet]
        [Route("Details")]
        public string Details()
        {
            return "Authenticated with JWT";
        }

       

        [HttpPost]
        public string AddUser(UserVM user )
        {
            if (user != null)
            {
                var userDetail = new Users
                {
                    Username = user.Username,
                    Password = user.Password,
                };              
                _Dbcontext.UserDetails.Add(userDetail);               
                _Dbcontext.SaveChanges();

                var addedUser = _Dbcontext.UserDetails.FirstOrDefault(u => u.Username == user.Username);

                if (addedUser != null)
                {
                    
                    var update = new UserRole
                    {
                        RoleId = user.RoleId,
                        UserId = addedUser.Id 
                    };

                    _Dbcontext.UserRole.Add(update);
                    _Dbcontext.SaveChanges();

                   
                }
                else
                {
                    return "User not found.";
                }
            }
           
        
            
            return "User added with Username " + user.Username +" And Password " + user.Password;
        }
    }
}
