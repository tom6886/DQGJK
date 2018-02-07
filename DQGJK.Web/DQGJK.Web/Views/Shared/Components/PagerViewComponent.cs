using DQGJK.Web.PageModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DQGJK.Web.Views.Shared.Components
{
    [ViewComponent(Name = "Pager")]
    public class PagerViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Pager pager)
        {
            await Task.Run(() => { });

            return View(pager);
        }
    }
}
