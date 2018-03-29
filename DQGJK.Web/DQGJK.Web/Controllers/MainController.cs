using DQGJK.Models;
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
            DateTime board = DateTime.Now - new TimeSpan(0, 10, 0);
            return _context.Station.Where(q => q.ModifyTime > board).Count();
        }

        [HttpPost]
        public PartialViewResult OverTime()
        {
            DateTime board = DateTime.Now - new TimeSpan(0, 10, 0);

            List<Station> list = _context.Station.Where(q => q.ModifyTime < board).OrderByDescending(q => q.ModifyTime).ToList();

            return PartialView("OverTime", list);
        }

        [HttpPost]
        public PartialViewResult Temperature()
        {
            List<CabinetInfo> list = _context.CabinetInfo.Where(q => q.TemperatureAlarm > 0).OrderByDescending(q => q.ModifyTime).ToList();

            return PartialView("Temperature", list);
        }

        [HttpPost]
        public PartialViewResult Humidity()
        {
            List<CabinetInfo> list = _context.CabinetInfo.Where(q => q.HumidityAlarm > 0).OrderByDescending(q => q.ModifyTime).ToList();

            return PartialView("Humidity", list);
        }
    }
}