namespace votegdgc.Models
{
    public class Vote
    {
        public int Id { get; set; }

        // Foreign keys
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public DateTime VotedAt { get; set; } = DateTime.UtcNow;
    }
}
