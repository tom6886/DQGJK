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
        public JsonResult GetOnlineCount()
        {
            Department department = HttpContext.Session.Get<Department>("SESSION-DEPARTMENT-KEY");

            DateTime board = DateTime.Now - new TimeSpan(0, 10, 0);

            var query = _context.Station.AsQueryable();
            var cabinetsQ = _context.CabinetInfo.AsQueryable();

            query = query.Where(q => q.Status == Status.enable);

            if (department != null)
            {
                query = query.Where(q => q.DeptID.Equals(department.ID));
                cabinetsQ = cabinetsQ.Where(q => q.DeptID.Equals(department.ID));
            }

            return Json(new
            {
                online = query.Where(q => q.ModifyTime > board).Count(),
                overtime = query.Where(q => q.ModifyTime < board).Count(),
                temperature = cabinetsQ.Where(q => q.TemperatureAlarm > 0).Count(),
                humidity = cabinetsQ.Where(q => q.HumidityAlarm > 0).Count()
            });
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