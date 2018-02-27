using DQGJK.Models;
using DQGJK.Web.Contexts;
using DQGJK.Web.PageModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace DQGJK.Web.Controllers
{
    public class MapController : BaseController
    {
        private DBContext _context;

        public MapController(DBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult StationList(string province, string city, string country, string station, int pi = 1)
        {
            Department department = HttpContext.Session.Get<Department>("SESSION-DEPARTMENT-KEY");

            var query = _context.StationInfo.AsQueryable();

            if (department != null) { query = query.Where(q => q.DeptID.Equals(department.ID)); }

            if (!string.IsNullOrEmpty(province)) { query = query.Where(q => q.Province.Equals(province)); }
            if (!string.IsNullOrEmpty(city)) { query = query.Where(q => q.City.Equals(city)); }
            if (!string.IsNullOrEmpty(country)) { query = query.Where(q => q.Country.Equals(country)); }
            if (!string.IsNullOrEmpty(station)) { query = query.Where(q => q.ID.Equals(station)); }

            Pager pager = new Pager(query.Count(), pi);

            List<StationInfo> list = query.OrderByDescending(q => q.ModifyTime).Skip((pager.PageIndex - 1) * 10).Take(10).ToList();

            ViewBag.Pager = pager;

            return PartialView("List", list);
        }
    }
}