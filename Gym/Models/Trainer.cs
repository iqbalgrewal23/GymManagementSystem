namespace Gym.Models
{
    public class Trainer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }


        // Navigation
        public List<Member>? Members { get; set; }
    }
}
