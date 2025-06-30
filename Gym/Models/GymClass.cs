namespace Gym.Models
{
    public class GymClass
    {
        public int Id { get; set; }
        public string ClassName { get; set; }

        // M-M with Member
        public List<MemberClass>? MemberClasses { get; set; }
    }
}
