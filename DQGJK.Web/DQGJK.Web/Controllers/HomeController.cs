using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DQGJK.Web.PageModels;
using DQGJK.Models;
using DQGJK.Web.Contexts;
using System.Xml.Linq;
using System.Collections;
using Microsoft.AspNetCore.Hosting;

namespace DQGJK.Web.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        //public PartialViewResult CreateMenu()
        //{
        //    List<Pmenu> menus = new List<Pmenu>();

        //    Guser user = HttpContext.Session.Get<Guser>("SESSION-ACCOUNT-KEY");

        //    if (user == null || string.IsNullOrWhiteSpace(user.Roles))
        //    {
        //        return PartialView("menu", menus);
        //    }

        //    string currentRole = user.Roles.ToLower();
        //    string xmlPath = _host.WebRootPath + "/Views/Menus.xml";
        //    XDocument xml = XDocument.Load(_host.WebRootPath + "/Views/Menus.xml");

        //    if (!string.IsNullOrWhiteSpace(currentRole) && xml != null && xml.Nodes().Count() > 0)
        //    {
        //        foreach (XElement element in xml.Root.Elements("menu").ToList())
        //        {
        //            string eRole = element.Attribute("roles").Value;
        //            if (string.IsNullOrWhiteSpace(eRole) || eRole.ToLower().Split(',').Contains(currentRole))
        //            {
        //                Pmenu menu = new Pmenu();
        //                menu.Name = element.Element("name").Value;
        //                menu.Href = element.Element("href").Value;
        //                menu.Iclass = element.Element("iclass").Value;

        //                XElement subEle = element.Element("subMenus");
        //                if (subEle != null && subEle.Nodes().Count() > 0)
        //                {
        //                    menu.SubMenuStyle = subEle.Element("style").Value;
        //                    menu.SubMenus = new Dictionary<string, string>();

        //                    ArrayList subMenus = new ArrayList();
        //                    foreach (XElement child in subEle.Elements("subMenu").ToList())
        //                    {
        //                        string cRole = child.Attribute("roles").Value;
        //                        if (string.IsNullOrWhiteSpace(cRole) || cRole.ToLower().Split(',').Contains(currentRole))
        //                        {
        //                            menu.SubMenus.Add(child.Element("name").Value, child.Element("href").Value);
        //                        }
        //                    }

        //                }

        //                menus.Add(menu);
        //            }
        //        }
        //    }
        //    return PartialView("menu", menus);
        //}
    }
}