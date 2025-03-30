using Microsoft.AspNetCore.Identity;

namespace OkeiDormitory.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public Role Role { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
