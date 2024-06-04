using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentDetails.Data;
using StudentDetails.Models;
using StudentDetails.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace StudentDetails.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;

        private readonly ApplicationDbContext _Dbcontext;

        public LoginController(IConfiguration configuration, ApplicationDbContext Dbcontext)
        {
            _config = configuration;
            _Dbcontext = Dbcontext;
        }

        private Users? AuthenticateUser(LoginUser user)
        {
            Users _user = null;
            var userFromDatabase = _Dbcontext.UserDetails.FirstOrDefault(u => u.Username == user.Username);
            if (VerifyUsername(user.Username, userFromDatabase.Username) && VerifyPassword(user.Password, userFromDatabase.Password))
            {
                var addedUser = _Dbcontext.UserDetails.Include(s => s.Roles).ThenInclude(r => r.Role).FirstOrDefault(u => u.Username == user.Username);
                var RoleId = addedUser.Roles.FirstOrDefault().RoleId;


                _user = addedUser;
            }


            //var userFromDatabase = _Dbcontext.UserDetails.FirstOrDefault(u => u.Username == user.Username);

            //if (userFromDatabase != null && VerifyPassword(user.Password, userFromDatabase.Password))
            //{

            //    _user = userFromDatabase;
            //}

            return _user;
        }



        //private bool VerifyRole(IList<UserRole> roles1, IList<UserRole> roles2)
        //{
        //    throw new NotImplementedException();
        //}

        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {

            return enteredPassword == storedPassword;
        }

        private bool VerifyUsername(string enteredUsername, string storedUsername)
        {

            return enteredUsername == storedUsername;
        }

        private string GenerateToken(Users users)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("UserName", users.Username),
                new Claim("Password", users.Password),
                new Claim("RoleId", users!.Roles!.FirstOrDefault()!.RoleId!.ToString()),
                new Claim(ClaimTypes.Role, users!.Roles!.FirstOrDefault()!.Role.RoleName!.ToString()),

            };


            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims
                , expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginUser user)
        {
            try
            {
                var user_ = AuthenticateUser(user);
                if (user_ != null)
                {
                    var RoleID = user_.Roles;
                    var token = GenerateToken(user_);
                    var response = new
                    {
                        token = token,
                        username = user_.Username
                    };

                    return Ok(response);
                }
            }
            catch (Exception)
            {
            }

            // return response;
            return BadRequest(new { message = "User Data is not valid !" });

        }


    }
}
