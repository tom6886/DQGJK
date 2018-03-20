using DQGJK.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace DQGJK.Web.Controllers
{
    public class HomeController : BaseController
    {
        private DBContext _context;

        public HomeController(DBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public PartialViewResult Carousel(string stationCode)
        {
            if (string.IsNullOrEmpty(stationCode)) { stationCode = "000000000009"; }

            ViewBag.station = _context.Station.Where(q => q.Code.Equals(stationCode)).FirstOrDefault();

            List<Cabinet> list = _context.Cabinet.Where(q => q.StationCode.Equals(stationCode)).ToList();

            return PartialView("List", list);
        }
    }
}