using System.ComponentModel.DataAnnotations;

namespace OkeiDormitory.Models.Entities
{
    public class Room
    {
        [Key]
        public int Number { get; set; }
    }
}
