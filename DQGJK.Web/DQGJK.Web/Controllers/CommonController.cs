using DQGJK.Models;
using DQGJK.Web.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DQGJK.Web.Controllers
{
    public class CommonController : Controller
    {
        private DBContext _context;

        private IDistributedCache _memoryCache;

        public CommonController(DBContext context, IDistributedCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public JsonResult getDept(int deptType, string pId, string key, int page = 1)
        {
            Department department = HttpContext.Session.Get<Department>("SESSION-DEPARTMENT-KEY");

            var query = _context.Department.AsQueryable();

            query = query.Where(q => q.Status == Status.enable);

            if (deptType == 0)
            {
                query = query.Where(q => q.ParentID == null);
            }
            else
            {
                if (department == null)
                {
                    if (string.IsNullOrEmpty(pId)) { query = query.Where(q => 1 != 1); }
                    else { query = query.Where(q => q.ParentID.Equals(pId)); }
                }
                else
                {
                    query = query.Where(q => q.ParentID.Equals(department.ID));
                }
            }

            if (!string.IsNullOrEmpty(key)) { query = query.Where(q => q.Name.Contains(key) || q.Code.Contains(key)); }

            ArrayList results = new ArrayList();

            List<Department> list = query.OrderBy(q => q.Code).Skip((page - 1) * 10).Take(10).ToList();

            foreach (var item in list)
            {
                results.Add(new { id = item.ID, name = item.Name });
            }

            int total = query.Count();

            return Json(new { results = results, total = total, pageSize = 10 });
        }

        [HttpGet]
        public JsonResult getStation(string key, int page = 1)
        {
            Department department = HttpContext.Session.Get<Department>("SESSION-DEPARTMENT-KEY");

            var query = _context.Station.AsQueryable();

            query = query.Where(q => q.Status == Status.enable);

            //if (department != null) { query = query.Where(q => q.DepartmentID.Equals(department.ID)); }

            if (!string.IsNullOrEmpty(key)) { query = query.Where(q => q.Name.Contains(key) || q.Code.Contains(key)); }

            ArrayList results = new ArrayList();

            List<Station> list = query.OrderBy(q => q.Code).Skip((page - 1) * 10).Take(10).ToList();

            foreach (var item in list)
            {
                results.Add(new { id = item.ID, name = item.Name });
            }

            int total = query.Count();

            return Json(new { results = results, total = total, pageSize = 10 });
        }

        [HttpGet]
        public JsonResult getRoles(string key, int page = 1)
        {
            ArrayList results = new ArrayList();

            Dictionary<string, string> _roles = _memoryCache.Get<Dictionary<string, string>>("Roles");

            List<KeyValuePair<string, string>> roles = (from q in _roles
                                                        select q).Skip(page - 1).Take(10).ToList();

            foreach (var item in roles)
            {
                if (item.Key == "Administrator") { continue; }

                results.Add(new { id = item.Key, name = item.Value });
            }

            int total = _roles.Count();

            return Json(new { results = results, total = total, pageSize = 10 });
        }

        [HttpGet]
        public JsonResult getUser(string pId, string key, int page = 1)
        {
            var query = _context.Guser.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pId)) { query = query.Where(q => q.DeptID.Equals(pId)); }
            if (!string.IsNullOrEmpty(key)) { query = query.Where(q => q.DisplayName.Contains(key) || q.DisplayName.Contains(key)); }

            ArrayList results = new ArrayList();

            List<Guser> list = query.OrderBy(q => q.DisplayName).Skip((page - 1) * 10).Take(10).ToList();

            foreach (var item in list)
            {
                results.Add(new { id = item.ID, name = item.DisplayName });
            }

            int total = query.Count();

            return Json(new { results = results, total = total, pageSize = 10 });
        }
    }
}