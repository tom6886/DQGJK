using DQGJK.Models;
using DQGJK.Web.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public PartialViewResult Carousel(string stationCode)
        {
            if (string.IsNullOrEmpty(stationCode))
            {
                Cabinet cabinet = _context.Cabinet.OrderBy(q => q.ModifyTime).FirstOrDefault();
                if (cabinet != null) { stationCode = cabinet.StationCode; }
            }

            ViewBag.user = HttpContext.Session.Get<Guser>("SESSION-ACCOUNT-KEY");

            ViewBag.station = _context.Station.Where(q => q.Code.Equals(stationCode)).FirstOrDefault();

            List<Cabinet> list = _context.Cabinet.Where(q => q.StationCode.Equals(stationCode)).ToList();

            return PartialView("List", list);
        }

        [HttpPost]
        public JsonResult Command(string stationCode, string functionCode, DeviceOperate operate)
        {
            Guser user = HttpContext.Session.Get<Guser>("SESSION-ACCOUNT-KEY");

            if (!user.Roles.Equals("Administrator")) { return Json(new { code = -1, msg = "只有管理员角色可以遥控设备" }); }

            Operate parent = _context.Operate.Where(q => q.ClientCode.Equals(stationCode)
                                && q.FunctionCode.Equals(functionCode) && q.State == OperateState.Before).FirstOrDefault();

            if (parent == null)
            {
                parent = new Operate
                {
                    ClientCode = stationCode,
                    FunctionCode = functionCode
                };
                _context.Operate.Add(parent);

                DeviceOperate newOperate = new DeviceOperate(parent.ID, operate);
                _context.DeviceOperate.Add(newOperate);
            }
            else
            {
                DeviceOperate oldOperate = _context.DeviceOperate.Where(q => q.OperateID.Equals(parent.ID)
                                            && q.DeviceCode.Equals(operate.DeviceCode)).FirstOrDefault();

                if (oldOperate == null)
                {
                    DeviceOperate newOperate = new DeviceOperate(parent.ID, operate);
                    _context.DeviceOperate.Add(newOperate);
                }
                else
                {
                    oldOperate.Update(operate);
                    _context.Entry(oldOperate).State = EntityState.Modified;
                }
            }

            _context.SaveChanges();

            return Json(new { code = 1, msg = "命令已下发" });
        }
    }
}