using DQGJK.Models;
using DQGJK.Web.Contexts;
using DQGJK.Web.PageModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace DQGJK.Web.Controllers
{
    public class UserController : BaseController
    {
        private DBContext _context;

        public UserController(DBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult List(string key, int pi = 1)
        {
            Department department = HttpContext.Session.Get<Department>("SESSION-ACCOUNT-KEY");

            var query = _context.UserInfo.AsQueryable();

            if (department != null) { query = query.Where(q => q.DwID.Equals(department.ID)); }

            if (!string.IsNullOrEmpty(key)) { query = query.Where(q => q.DisplayName.Contains(key) || q.Account.Contains(key)); }

            Pager pager = new Pager(query.Count(), pi);

            List<UserInfo> list = query.OrderByDescending(q => q.ModifyTime).Skip((pager.PageIndex - 1) * 10).Take(10).ToList();

            //list.ForEach(u =>
            //{
            //    u.Roles = !string.IsNullOrEmpty(u.Roles) && UserContext.roles.Keys.Contains(u.Roles) ? UserContext.roles[u.Roles] : string.Empty;
            //});

            ViewBag.Pager = pager;

            return PartialView("List", list);
        }
    }
}