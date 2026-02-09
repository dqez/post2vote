using Microsoft.AspNetCore.Mvc;
using votegdgc.ViewModels;

namespace votegdgc.ViewComponents
{
    public class LeaderboardItemViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(LeaderboardItemViewModel model)
        {
            return View(model);
        }
    }
}
