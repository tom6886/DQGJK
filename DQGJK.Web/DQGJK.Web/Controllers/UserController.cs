using DQGJK.Models;
using DQGJK.Web.Contexts;
using DQGJK.Web.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace DQGJK.Web.Controllers
{
    public class UserController : BaseController
    {
        private DBContext _context;

        private IDistributedCache _memoryCache;

        public UserController(DBContext context, IDistributedCache memoryCache)
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

            var query = _context.UserInfo.AsQueryable();

            if (department != null) { query = query.Where(q => q.DwID.Equals(department.ID)); }

            if (!string.IsNullOrEmpty(key)) { query = query.Where(q => q.DisplayName.Contains(key) || q.Account.Contains(key)); }

            Pager pager = new Pager(query.Count(), pi);

            List<UserInfo> list = query.OrderByDescending(q => q.ModifyTime).Skip((pager.PageIndex - 1) * 10).Take(10).ToList();

            Dictionary<string, string> roles = _memoryCache.Get<Dictionary<string, string>>("Roles");

            list.ForEach(u =>
            {
                u.Roles = !string.IsNullOrEmpty(u.Roles) && roles.Keys.Contains(u.Roles) ? roles[u.Roles] : string.Empty;
            });

            ViewBag.Pager = pager;

            return PartialView("List", list);
        }

        [HttpPost]
        public object Dialog(string userID)
        {
            ViewBag.userDept = HttpContext.Session.Get<Department>("SESSION-DEPARTMENT-KEY");

            if (!string.IsNullOrEmpty(userID))
            {
                Guser account = _context.Guser.Where(q => q.ID.Equals(userID)).FirstOrDefault();

                if (account == null) { return Json(new { code = -1, msg = "该用户不存在" }); }

                Dictionary<string, string> roles = _memoryCache.Get<Dictionary<string, string>>("Roles");

                ViewBag.account = account;
                Department depart = string.IsNullOrEmpty(account.DeptID) ? new Department() : _context.Department.Where(q => q.ID.Equals(account.DeptID)).FirstOrDefault();
                ViewBag.Dept = depart;
                ViewBag.Dw = string.IsNullOrEmpty(depart.ParentID) ? new Department() : _context.Department.Where(q => q.ID.Equals(depart.ParentID)).FirstOrDefault();
                ViewBag.RoleName = !string.IsNullOrEmpty(account.Roles) && roles.Keys.Contains(account.Roles) ? roles[account.Roles] : string.Empty;

                return PartialView("Edit");
            }

            return PartialView("Add");
        }

        [HttpPost]
        public JsonResult Edit(Guser user)
        {
            Guser oldUser = _context.Guser.Where(q => q.ID.Equals(user.ID)).FirstOrDefault();

            if (oldUser == null)
            {
                Guser currentUser = HttpContext.Session.Get<Guser>("SESSION-ACCOUNT-KEY");

                user.PassWord = StringUtil.Md5Encrypt(user.PassWord);
                user.CreatorID = currentUser.ID;
                user.Creator = currentUser.DisplayName;
                user.Status = Status.enable;

                _context.Guser.Add(user);
            }
            else
            {
                oldUser.DisplayName = user.DisplayName;
                oldUser.Tel = user.Tel;
                oldUser.ModifyTime = DateTime.Now;
                oldUser.Status = user.Status;
                oldUser.DeptID = user.DeptID;
                oldUser.Roles = user.Roles;

                _context.Entry(oldUser).State = EntityState.Modified;
            }
            _context.SaveChanges();

            return Json(new { code = 1, msg = "保存成功" });
        }

        [HttpPost]
        public JsonResult Delete(string userID)
        {
            Guser account = _context.Guser.Where(q => q.ID.Equals(userID)).FirstOrDefault();

            if (account == null) { return Json(new { code = -1, msg = "您要删除的用户不存在" }); }

            _context.Guser.Remove(account);

            _context.SaveChanges();

            return Json(new { code = 1, msg = "操作成功" });
        }
    }
}