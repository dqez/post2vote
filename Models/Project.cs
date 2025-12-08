namespace votegdgc.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Prompt { get; set; } = string.Empty;
        public string ProjectLink { get; set; } = string.Empty;
        public string ThumbnailLink { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // Navigation properties
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();

        // Computed property: Tổng số vote
        public int VoteCount => Votes?.Count ?? 0;
    }
}
