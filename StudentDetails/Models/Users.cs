using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace StudentDetails.Models
{
    public class Users
    {
        public long Id { get; set; }
        public string Username { get; set; }
       
        public string Password { get; set; }             
        public IList<UserRole> Roles { get; set; } = new List<UserRole>();
        
    }
}
