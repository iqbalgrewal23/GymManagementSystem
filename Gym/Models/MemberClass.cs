namespace Gym.Models
{
    public class MemberClass
    {
        public int MemberId { get; set; }
        public Member Member { get; set; }

        public int GymClassId { get; set; }
        public GymClass GymClass { get; set; }
    }
}
