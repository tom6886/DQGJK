using DQGJK.Models;
using DQGJK.Web.Contexts;
using DQGJK.Web.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DQGJK.Web.Controllers
{
    public class CabinetController : BaseController
    {
        private DBContext _context;

        public CabinetController(DBContext context)
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
            Department department = HttpContext.Session.Get<Department>("SESSION-DEPARTMENT-KEY");

            var query = _context.CabinetInfo.AsQueryable();

            if (department != null) { query = query.Where(q => q.DeptID.Equals(department.ID)); }

            if (!string.IsNullOrEmpty(key)) { query = query.Where(q => q.StationCode.Equals(key)); }

            Pager pager = new Pager(query.Count(), pi);

            List<CabinetInfo> list = query.OrderBy(q => q.Code).OrderBy(q => q.StationCode).Skip((pager.PageIndex - 1) * 10).Take(10).ToList();

            ViewBag.Pager = pager;

            return PartialView("List", list);
        }

        [HttpPost]
        public object Dialog(string cabinetID)
        {
            ViewBag.userDept = HttpContext.Session.Get<Department>("SESSION-DEPARTMENT-KEY");

            if (string.IsNullOrEmpty(cabinetID)) { return PartialView("Add"); }

            CabinetInfo cabinet = _context.CabinetInfo.Where(q => q.ID.Equals(cabinetID)).FirstOrDefault();

            if (cabinet == null) { return Json(new { code = -1, msg = "找不到指定的电气柜" }); }

            ViewBag.cabinet = cabinet;

            return PartialView("Edit");
        }

        [HttpPost]
        public JsonResult Edit(Cabinet cabinet)
        {
            Cabinet oldCab = _context.Cabinet.Where(q => q.ID.Equals(cabinet.ID)).FirstOrDefault();

            if (oldCab == null)
            {
                Guser user = HttpContext.Session.Get<Guser>("SESSION-ACCOUNT-KEY");

                cabinet.CreatorID = user.ID;
                cabinet.Creator = user.DisplayName;
                cabinet.Status = Status.enable;

                _context.Cabinet.Add(cabinet);
            }
            else
            {
                oldCab.ModifyTime = DateTime.Now;
                oldCab.StationCode = oldCab.StationCode;
                oldCab.Name = cabinet.Name;
                oldCab.Sort = cabinet.Sort;
                //oldCab.Status = cabinet.Status;

                _context.Entry(oldCab).State = EntityState.Modified;
            }

            _context.SaveChanges();

            return Json(new { code = 1, msg = "保存成功" });
        }

        [HttpPost]
        public JsonResult Delete(string cabinetID)
        {
            Cabinet cab = _context.Cabinet.Where(q => q.ID.Equals(cabinetID)).FirstOrDefault();

            if (cab == null) { return Json(new { code = -1, msg = "您要删除的设备不存在" }); }

            _context.Cabinet.Remove(cab);

            _context.SaveChanges();

            return Json(new { code = 1, msg = "删除成功" });
        }
    }
}