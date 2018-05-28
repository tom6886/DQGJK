using DQGJK.Models;
using DQGJK.Web.Contexts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DQGJK.Web.Controllers
{
    public class MainController : Controller
    {
        private DBContext _context;

        public MainController(DBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public int GetOnlineCount()
        {
            Department department = HttpContext.Session.Get<Department>("SESSION-DEPARTMENT-KEY");

            DateTime board = DateTime.Now - new TimeSpan(0, 10, 0);

            var query = _context.Station.AsQueryable();

            query = query.Where(q => q.Status == Status.enable && q.ModifyTime > board);

            if (department != null) { query = query.Where(q => q.DeptID.Equals(department.ID)); }

            return query.Count();
        }

        [HttpPost]
        public PartialViewResult OverTime()
        {
            Department department = HttpContext.Session.Get<Department>("SESSION-DEPARTMENT-KEY");

            DateTime board = DateTime.Now - new TimeSpan(0, 10, 0);

            var query = _context.Station.AsQueryable();

            query = query.Where(q => q.Status == Status.enable && q.ModifyTime < board);

            if (department != null) { query = query.Where(q => q.DeptID.Equals(department.ID)); }

            List<Station> list = query.OrderByDescending(q => q.ModifyTime).ToList();

            return PartialView("OverTime", list);
        }

        [HttpPost]
        public PartialViewResult Temperature()
        {
            Department department = HttpContext.Session.Get<Department>("SESSION-DEPARTMENT-KEY");

            var query = _context.CabinetInfo.AsQueryable();

            if (department != null) { query = query.Where(q => q.DeptID.Equals(department.ID)); }

            List<CabinetInfo> list = query.Where(q => q.TemperatureAlarm > 0).OrderByDescending(q => q.ModifyTime).ToList();

            return PartialView("Temperature", list);
        }

        [HttpPost]
        public PartialViewResult Humidity()
        {
            Department department = HttpContext.Session.Get<Department>("SESSION-DEPARTMENT-KEY");

            var query = _context.CabinetInfo.AsQueryable();

            if (department != null) { query = query.Where(q => q.DeptID.Equals(department.ID)); }

            List<CabinetInfo> list = query.Where(q => q.HumidityAlarm > 0).OrderByDescending(q => q.ModifyTime).ToList();

            return PartialView("Humidity", list);
        }
    }
}