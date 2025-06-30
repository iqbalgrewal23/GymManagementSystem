namespace Gym.DTOs
{
    public class MemberDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int? TrainerId { get; set; }
        public List<int>? GymClassIds { get; set; }
    }
}
