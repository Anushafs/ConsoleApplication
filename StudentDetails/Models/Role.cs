using Microsoft.AspNetCore.Identity;

namespace StudentDetails.Models
{
    public class Role
    {
        public long Id { get; set; }
        public string RoleName { get; set; }
        public IList<UserRole> Users { get; set; } = new List<UserRole>();

    }
}
