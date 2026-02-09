using votegdgc.Models;

namespace votegdgc.ViewModels
{
    public class LeaderboardViewModel
    {
        public IEnumerable<Project> Projects { get; set; } = new List<Project>();
        public User? CurrentUser { get; set; }
        
        public IEnumerable<LeaderboardItemViewModel> GetLeaderboardItems()
        {
            int rank = 1;
            return Projects.Select(p => new LeaderboardItemViewModel
            {
                Project = p,
                Rank = rank++
            });
        }
    }
    
    public class LeaderboardItemViewModel
    {
        public Project Project { get; set; } = null!;
        public int Rank { get; set; }
        
        public string GetCardClass()
        {
            string baseClass = "relative flex items-center gap-4 p-4 border-b-4 border-r-4 border-l-2 border-t-2 transition-all";
            
            return Rank switch
            {
                1 => $"{baseClass} border-gdg-yellow bg-gdg-yellow/10 scale-105 z-10",
                2 => $"{baseClass} border-gray-400 bg-gray-100",
                3 => $"{baseClass} border-orange-400 bg-orange-50",
                _ => $"{baseClass} border-black bg-white"
            };
        }
        
        public string GetRankIconSvg()
        {
            return Rank switch
            {
                1 => @"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"" class=""lucide lucide-crown w-6 h-6 text-gdg-yellow fill-black""><path d=""m2 4 3 12h14l3-12-6 7-4-7-4 7-6-7zm3 16h14""></path></svg>",
                2 or 3 => @"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"" class=""lucide lucide-medal w-6 h-6 " + (Rank == 2 ? "text-gray-500" : "text-orange-600") + @"""><path d=""M7.21 15 2.66 7.14a2 2 0 0 1 .13-2.2L4.4 2.8A2 2 0 0 1 6 2h12a2 2 0 0 1 1.6.8l1.6 2.14a2 2 0 0 1 .14 2.2L16.79 15""></path><path d=""M11 12 5.12 2.2""></path><path d=""m13 12 5.88-9.8""></path><path d=""M8 7h8""></path><circle cx=""12"" cy=""17"" r=""5""></circle><path d=""M12 18v-2h-.5""></path></svg>",
                _ => $@"<span class=""font-black text-xl"">{Rank}</span>"
            };
        }
    }
}
