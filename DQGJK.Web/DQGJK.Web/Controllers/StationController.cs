﻿using DQGJK.Models;
using DQGJK.Web.Contexts;
using DQGJK.Web.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DQGJK.Web.Controllers
{
    public class StationController : BaseController
    {
        private DBContext _context;

        private IDistributedCache _memoryCache;

        public StationController(DBContext context, IDistributedCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult List(string key, int pi = 1)
        {
            Department department = HttpContext.Session.Get<Department>("SESSION-DEPARTMENT-KEY");

            var query = _context.StationInfo.AsQueryable();

            if (department != null) { query = query.Where(q => q.DeptID.Equals(department.ID)); }

            if (!string.IsNullOrEmpty(key)) { query = query.Where(q => q.Name.Contains(key) || q.Code.Contains(key)); }

            Pager pager = new Pager(query.Count(), pi);

            List<StationInfo> list = query.OrderByDescending(q => q.ModifyTime).Skip((pager.PageIndex - 1) * 10).Take(10).ToList();

            ViewBag.Pager = pager;

            return PartialView("List", list);
        }

        [HttpPost]
        public object Dialog(string stationID)
        {
            ViewBag.userDept = HttpContext.Session.Get<Department>("SESSION-DEPARTMENT-KEY");

            if (string.IsNullOrEmpty(stationID)) { return PartialView("Add"); }

            Station station = _context.Station.Where(q => q.ID.Equals(stationID)).FirstOrDefault();

            if (station == null) { return Json(new { code = -1, msg = "找不到指定的站点" }); }

            ViewBag.station = station;

            Department dw = _context.Department.Where(q => q.ID.Equals(station.DeptID)).FirstOrDefault();

            ViewBag.Dw = dw;

            return PartialView("Edit");
        }

        [HttpPost]
        public JsonResult Edit(Station station)
        {
            Department department = HttpContext.Session.Get<Department>("SESSION-DEPARTMENT-KEY");

            Station oldStat = _context.Station.Where(q => q.ID.Equals(station.ID)).FirstOrDefault();

            if (oldStat == null)
            {
                if (department != null) { station.DeptID = department.ID; }

                station.Status = Status.enable;

                _context.Station.Add(station);
            }
            else
            {
                if (department == null)
                {
                    oldStat.DeptID = station.DeptID;
                }

                oldStat.ModifyTime = DateTime.Now;
                oldStat.Name = station.Name;
                oldStat.Province = station.Province;
                oldStat.City = station.City;
                oldStat.Country = station.Country;
                oldStat.CityCode = station.CityCode;
                oldStat.Address = station.Address;
                oldStat.Status = station.Status;

                _context.Entry(oldStat).State = EntityState.Modified;
            }

            _context.SaveChanges();

            return Json(new { code = 1, msg = "保存成功" });
        }

        [HttpPost]
        public JsonResult Delete(string stationID)
        {
            Station stat = _context.Station.Where(q => q.ID.Equals(stationID)).FirstOrDefault();

            if (stat == null) { return Json(new { code = -1, msg = "您要删除的站点不存在" }); }

            _context.Station.Remove(stat);

            _context.SaveChanges();

            return Json(new { code = 1, msg = "删除成功" });
        }
    }
}