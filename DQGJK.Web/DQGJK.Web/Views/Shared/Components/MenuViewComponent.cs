using DQGJK.Models;
using DQGJK.Web.Contexts;
using DQGJK.Web.PageModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DQGJK.Web.Views.Shared.Components
{
    [ViewComponent(Name = "Menu")]
    public class MenuViewComponent : ViewComponent
    {
        private IHostingEnvironment _host = null;

        public MenuViewComponent(IHostingEnvironment host)
        {
            this._host = host;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.Run(() => { });

            List<Pmenu> menus = new List<Pmenu>();

            Guser user = HttpContext.Session.Get<Guser>("SESSION-ACCOUNT-KEY");

            if (user == null || string.IsNullOrWhiteSpace(user.Roles))
            {
                return View(menus);
            }

            string currentRole = user.Roles.ToLower();
            XDocument xml = XDocument.Load(_host.ContentRootPath + "/Views/Menus.xml");

            if (!string.IsNullOrWhiteSpace(currentRole) && xml != null && xml.Nodes().Count() > 0)
            {
                foreach (XElement element in xml.Root.Elements("menu").ToList())
                {
                    string eRole = element.Attribute("roles").Value;
                    if (string.IsNullOrWhiteSpace(eRole) || eRole.ToLower().Split(',').Contains(currentRole))
                    {
                        Pmenu menu = new Pmenu();
                        menu.Name = element.Element("name").Value;
                        menu.Href = element.Element("href").Value;
                        menu.Iclass = element.Element("iclass").Value;

                        XElement subEle = element.Element("subMenus");
                        if (subEle != null && subEle.Nodes().Count() > 0)
                        {
                            menu.SubMenuStyle = subEle.Element("style").Value;
                            menu.SubMenus = new Dictionary<string, string>();

                            ArrayList subMenus = new ArrayList();
                            foreach (XElement child in subEle.Elements("subMenu").ToList())
                            {
                                string cRole = child.Attribute("roles").Value;
                                if (string.IsNullOrWhiteSpace(cRole) || cRole.ToLower().Split(',').Contains(currentRole))
                                {
                                    menu.SubMenus.Add(child.Element("name").Value, child.Element("href").Value);
                                }
                            }

                        }

                        menus.Add(menu);
                    }
                }
            }

            return View(menus);
        }
    }
}
