namespace Gym.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }


        // 1-M with Trainer
        public int? TrainerId { get; set; }
        public Trainer? Trainer { get; set; }

        // M-M with GymClass
        public List<MemberClass>? MemberClasses { get; set; }
    }
}
