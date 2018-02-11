using DQGJK.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace DQGJK.Web.Controllers
{
    public partial class DepartmentController : BaseController
    {
        [HttpPost]
        public object Nodes(string code)
        {
            ViewBag.code = code;

            return PartialView("Nodes");
        }

        [HttpGet]
        public ActionResult NodesList(string code)
        {
            List<Department> nodes = _context.Department.Where(q => q.ParentID.Equals(code)).OrderBy(q => q.Code).ToList();

            return PartialView("NodesList", nodes);
        }

        [HttpPost]
        public object NodesDialog(string parentID, string id)
        {
            Department parent = _context.Department.Where(q => q.ID.Equals(parentID)).FirstOrDefault();

            if (parent == null) { return Json(new { code = -1, msg = "找不到该单位" }); }

            ViewBag.parent = parent;

            if (!string.IsNullOrEmpty(id))
            {
                Department dept = _context.Department.Where(q => q.ID.Equals(id)).FirstOrDefault();

                if (dept == null) { return Json(new { code = -2, msg = "找不到指定参数" }); }

                ViewBag.dept = dept;

                return PartialView("NodesEdit");
            }

            return PartialView("NodesAdd");
        }
    }
}