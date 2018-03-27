using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DQGJK.Models;

namespace DQGJK.Web.Controllers
{
    public class TrendController : BaseController
    {
        private DBContext _context;

        public TrendController(DBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult StationInfo(string stationCode)
        {
            if (string.IsNullOrEmpty(stationCode))
            {
                Cabinet cabinet = _context.Cabinet.OrderBy(q => q.ModifyTime).FirstOrDefault();
                if (cabinet != null) { stationCode = cabinet.StationCode; }
            }

            ViewBag.station = _context.Station.Where(q => q.Code.Equals(stationCode)).FirstOrDefault();

            return PartialView("List");
        }
    }
}