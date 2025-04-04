using System.ComponentModel.DataAnnotations;

namespace OkeiDormitory.Models.Entities
{
    public class Room
    {
        [Key]
        public int Number { get; set; }
        public ICollection<Reward> Rewards { get; set; } = new List<Reward>();
        public ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();
    }
}
