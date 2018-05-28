using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DQGJK.Models;
using System.Collections;
using DQGJK.Web.PageModels;
using DQGJK.Web.Contexts;

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
            Department department = HttpContext.Session.Get<Department>("SESSION-DEPARTMENT-KEY");

            if (string.IsNullOrEmpty(stationCode))
            {
                var query = _context.CabinetInfo.AsQueryable();

                if (department != null) { query = query.Where(q => q.DeptID.Equals(department.ID)); }

                CabinetInfo cabinet = query.OrderByDescending(q => q.ModifyTime).FirstOrDefault();

                if (cabinet != null) { stationCode = cabinet.StationCode; }
            }

            ViewBag.station = _context.Station.Where(q => q.Code.Equals(stationCode)).FirstOrDefault();

            return PartialView("List");
        }

        [HttpPost]
        public JsonResult Cabinets(string start, string end, string stationCode, int type)
        {
            DateTime startDate = Convert.ToDateTime(start);
            DateTime endDate = Convert.ToDateTime(end);

            if (startDate > endDate) { return Json(new { code = -1, msg = "开始日期不能大于结束日期" }); }

            List<string> devices = _context.Cabinet.Where(q => q.StationCode.Equals(stationCode)).OrderBy(q => q.Sort).Select(q => q.Code).ToList();

            if (devices.Count == 0) { return Json(new { code = 0, msg = "查询的环网柜没有从机信息" }); }

            ArrayList array = new ArrayList();

            if (type == 0)
            {
                array = getCabinetsDataByDay(startDate, endDate, stationCode, devices);
            }
            else
            {
                array = getCabinetsDataByMonth(startDate, endDate, stationCode, devices);
            }


            return Json(new { code = 1, data = array });
        }

        private ArrayList getCabinetsDataByDay(DateTime startDate, DateTime endDate, string stationCode, List<string> devices)
        {
            List<CabinetData> datas = _context.CabinetData.Where(q => q.ClientCode.Equals(stationCode)
            && q.CreateTime > startDate && q.CreateTime < endDate).ToList();

            TimeSpan span = endDate - startDate;

            ArrayList array = new ArrayList();

            foreach (var item in devices)
            {
                Pchart _chart = new Pchart(item);

                for (int i = 0, length = span.Days; i < length; i++)
                {
                    DateTime _date = startDate.AddDays(i);
                    _chart.XAxis.Add(_date.ToShortDateString());
                    CabinetData _data = datas.Where(q => q.DeviceCode.Equals(item) && q.Year == _date.Year
                                        && q.Month == _date.Month && q.Day == _date.Day).FirstOrDefault();
                    if (_data == null)
                    {
                        _chart.Temperature.Add(null);
                        _chart.Humidity.Add(null);
                    }
                    else
                    {
                        _chart.Temperature.Add(_data.AverageTemperature);
                        _chart.Humidity.Add(_data.AverageHumidity);
                    }
                }

                array.Add(_chart);
            }

            return array;
        }

        private ArrayList getCabinetsDataByMonth(DateTime startDate, DateTime endDate, string stationCode, List<string> devices)
        {
            List<Pstatistic> datas = (from q in _context.CabinetData
                                      group q by new { q.DeviceCode, q.Year, q.Month } into g
                                      select new Pstatistic()
                                      {
                                          DeviceCode = g.Key.DeviceCode,
                                          Year = g.Key.Year,
                                          Month = g.Key.Month,
                                          AverageHumidity = g.Average(p => p.AverageHumidity),
                                          AverageTemperature = g.Average(p => p.AverageTemperature)
                                      }).ToList();

            int months = (endDate.Year - startDate.Year) * 12 + (endDate.Month - startDate.Month) + 1;

            ArrayList array = new ArrayList();

            foreach (var item in devices)
            {
                Pchart _chart = new Pchart(item);

                for (int i = 0; i < months; i++)
                {
                    DateTime _date = startDate.AddMonths(i);

                    _chart.XAxis.Add(_date.ToString("yyyy-MM"));

                    Pstatistic _data = datas.Where(q => q.DeviceCode.Equals(item) && q.Year == _date.Year
                                        && q.Month == _date.Month).FirstOrDefault();

                    if (_data == null)
                    {
                        _chart.Temperature.Add(null);
                        _chart.Humidity.Add(null);
                    }
                    else
                    {
                        _chart.Temperature.Add(_data.AverageTemperature);
                        _chart.Humidity.Add(_data.AverageHumidity);
                    }
                }

                array.Add(_chart);
            }

            return array;
        }
    }
}