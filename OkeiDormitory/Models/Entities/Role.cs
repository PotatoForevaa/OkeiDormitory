using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OkeiDormitory.Models.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }        
    }
}
