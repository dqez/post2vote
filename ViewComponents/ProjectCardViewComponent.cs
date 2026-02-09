using Microsoft.AspNetCore.Mvc;
using votegdgc.ViewModels;

namespace votegdgc.ViewComponents
{
    public class ProjectCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ProjectCardViewModel model)
        {
            return View(model);
        }
    }
}
