using Microsoft.AspNetCore.Identity;

namespace StudentDetails.Models
{
    public class UserRole
    { 
        public long Id { get; set; } 
        public long UserId { get; set;}
        public Users User { get; set; }
        public long RoleId { get; set; }
        public Role Role { get; set; }
    }
}