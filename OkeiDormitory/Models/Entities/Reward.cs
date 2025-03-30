namespace OkeiDormitory.Models.Entities
{
    public class Reward
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Room Room { get; set; }
        public DateTime AwardDate { get; set; }
        public string Description { get; set; }
    }
}
