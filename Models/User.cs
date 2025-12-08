namespace votegdgc.Models
{
    public class User
    {
        public int Id { get; set; }
        public string GoogleId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();

        // Computed property: Số vote còn lại
        public int RemainingVotes => 5 - (Votes?.Count ?? 0);
    }
}
