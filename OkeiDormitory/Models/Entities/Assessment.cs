namespace OkeiDormitory.Models.Entities
{
    public class Assessment
    {
        public int Id { get; set; }
        public Room Room { get; set; }
        public User Inspector { get; set; }
        public int Rating { get; set; }
        public List<Photo> Photos { get; set; }
    }
}
