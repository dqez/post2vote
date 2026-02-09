using votegdgc.Models;

namespace votegdgc.ViewModels
{
    public class VotePageViewModel
    {
        public IEnumerable<Project> Projects { get; set; } = new List<Project>();
        public User? CurrentUser { get; set; }
        
        public int VotesLeft => CurrentUser?.RemainingVotes ?? 0;
        public bool IsAuthenticated => CurrentUser != null;
        
        public IEnumerable<ProjectCardViewModel> GetProjectCards()
        {
            return Projects.Select(p => new ProjectCardViewModel
            {
                Project = p,
                CurrentUser = CurrentUser
            });
        }
    }
}
