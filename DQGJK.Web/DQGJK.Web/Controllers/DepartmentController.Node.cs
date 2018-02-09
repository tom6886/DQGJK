using DQGJK.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace DQGJK.Web.Controllers
{
    public partial class DepartmentController : Controller
    {
        [HttpPost]
        public object Nodes(string code)
        {
            ViewBag.code = code;

            return PartialView("Nodes");
        }

        [HttpPost]
        public ActionResult NodesGet(string code)
        {
            List<Department> nodes = _context.Department.Where(q => q.ParentID.Equals(code)).OrderBy(q => q.Code).ToList();

            return PartialView("NodesList", nodes);
        }


    }
}