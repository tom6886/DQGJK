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
    public class DepartmentController : Controller
    {
        private DBContext _context;

        public DepartmentController(DBContext context)
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
            var query = _context.Department.AsQueryable();

            query = query.Where(q => q.ParentID == null);

            if (!string.IsNullOrEmpty(key)) { query = query.Where(q => q.Name.Contains(key) || q.Code.Contains(key)); }

            Pager pager = new Pager(query.Count(), pi);

            List<Department> list = query.OrderByDescending(q => q.ModifyTime).Skip((pager.PageIndex - 1) * 10).Take(10).ToList();

            ViewBag.Pager = pager;

            return PartialView("List", list);
        }

        [HttpPost]
        public object Dialog(string deptID)
        {
            if (string.IsNullOrEmpty(deptID)) { return PartialView("Add"); }

            Department dept = _context.Department.Where(q => q.ID.Equals(deptID)).FirstOrDefault();

            if (dept == null) { return Json(new { code = -1, msg = "找不到指定的单位" }); }

            ViewBag.Dept = dept;

            return PartialView("Edit");
        }

        [HttpPost]
        public JsonResult Edit(Department dept)
        {
            Department oldDept = _context.Department.Where(q => q.ID.Equals(dept.ID)).FirstOrDefault();

            Guser user = HttpContext.Session.Get<Guser>("SESSION-ACCOUNT-KEY");

            if (oldDept == null)
            {
                dept.Creator = user.DisplayName;
                dept.CreatorID = user.ID;
                dept.Status = Status.enable;

                _context.Department.Add(dept);
            }
            else
            {
                oldDept.ModifyTime = DateTime.Now;
                oldDept.Name = dept.Name;

                //只有部门的编号可以修改
                if (!string.IsNullOrEmpty(oldDept.ParentID))
                {
                    oldDept.Code = dept.Code;
                }

                oldDept.Status = dept.Status;

                _context.Entry(oldDept).State = EntityState.Modified;
            }
            _context.SaveChanges();

            return Json(new { code = 1, msg = "保存成功" });
        }

        [HttpPost]
        public JsonResult Delete(string deptID)
        {
            Department dept = _context.Department.Where(q => q.ID.Equals(deptID)).FirstOrDefault();

            if (dept == null) { return Json(new { code = -1, msg = "您要删除的组织机构不存在" }); }

            //if (!string.IsNullOrEmpty(dept.ParentID))
            //{
            //    List<Station> stats = _context.Station.Where(q => q.DeptID.Equals(dept.ID)).ToList();

            //    if (stats.Count > 0)
            //    {
            //        List<string> statsIds = stats.Select(q => q.ID).ToList();

            //        List<Cabinet> devices = _context.Cabinet.Where(q => statsIds.Contains(q.StationID)).ToList();

            //        if (devices.Count > 0) { Device.Delete(devices, db); }

            //        _context.Station.RemoveRange(stats);
            //    }
            //}

            _context.Department.Remove(dept);

            _context.SaveChanges();

            return Json(new { code = 1, msg = "删除成功" });
        }
    }
}