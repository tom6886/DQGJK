using DQGJK.Models;
using DQGJK.Web.Contexts;
using DQGJK.Web.PageModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DQGJK.Web.Controllers
{
    public class HistoryController : BaseController
    {
        private DBContext _context;

        public HistoryController(DBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult List(string province, string city, string country, string stationID, string startDate, string endDate, int pi = 1)
        {
            Department department = HttpContext.Session.Get<Department>("SESSION-DEPARTMENT-KEY");

            var query = _context.CabinetDataInfo.AsQueryable();

            if (department != null) { query = query.Where(q => q.DeptID.Equals(department.ID)); }
            if (!string.IsNullOrEmpty(province)) { query = query.Where(q => q.Province.Equals(province)); }
            if (!string.IsNullOrEmpty(city)) { query = query.Where(q => q.City.Equals(city)); }
            if (!string.IsNullOrEmpty(country)) { query = query.Where(q => q.Country.Equals(country)); }
            if (!string.IsNullOrEmpty(stationID)) { query = query.Where(q => q.StationID.Equals(stationID)); }
            if (!string.IsNullOrEmpty(startDate)) { query = query.Where(q => q.CreateTime >= Convert.ToDateTime(startDate)); }
            if (!string.IsNullOrEmpty(endDate)) { query = query.Where(q => q.CreateTime < Convert.ToDateTime(endDate)); }

            Pager pager = new Pager(query.Count(), pi);

            List<CabinetDataInfo> list = query.OrderByDescending(q => q.CreateTime).Skip((pager.PageIndex - 1) * 10).Take(10).ToList();

            ViewBag.Pager = pager;

            return PartialView("List", list);
        }
    }
}