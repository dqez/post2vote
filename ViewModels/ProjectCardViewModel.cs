using votegdgc.Models;

namespace votegdgc.ViewModels
{
    public class ProjectCardViewModel
    {
        public Project Project { get; set; } = null!;
        public User? CurrentUser { get; set; }
        
        public bool IsOwner => CurrentUser != null && Project.UserId == CurrentUser.Id;
        public bool HasVoted => CurrentUser != null && Project.Votes.Any(v => v.UserId == CurrentUser.Id);
        public bool CanVote => CurrentUser != null && !IsOwner && !HasVoted && CurrentUser.RemainingVotes > 0;
        public bool NoVotesLeft => CurrentUser != null && CurrentUser.RemainingVotes <= 0;
        
        public string GetVoteButtonClass()
        {
            if (HasVoted)
                return "w-full relative inline-flex items-center justify-center font-bold border-2 border-black transition-all active:translate-x-[2px] active:translate-y-[2px] active:shadow-none disabled:opacity-50 disabled:cursor-not-allowed bg-gdg-red text-black shadow-neo hover:-translate-y-0.5 hover:shadow-[6px_6px_0px_0px_#000] px-6 py-3 text-base";
            if (CanVote)
                return "w-full relative inline-flex items-center justify-center font-bold border-2 border-black transition-all active:translate-x-[2px] active:translate-y-[2px] active:shadow-none disabled:opacity-50 disabled:cursor-not-allowed bg-gdg-blue text-white shadow-neo hover:-translate-y-0.5 hover:shadow-[6px_6px_0px_0px_#000] px-6 py-3 text-base flex-1";
            
            return "relative inline-flex items-center justify-center font-bold border-2 border-black transition-all active:translate-x-[2px] active:translate-y-[2px] active:shadow-none disabled:opacity-50 disabled:cursor-not-allowed bg-gray-200 text-gray-500 shadow-neo px-6 py-3 text-base flex-1";
        }
        
        public string GetVoteButtonText()
        {
            if (HasVoted) return "Unvote";
            if (IsOwner) return "Owner";
            if (NoVotesLeft) return "No Votes";
            return "Vote";
        }
    }
}
