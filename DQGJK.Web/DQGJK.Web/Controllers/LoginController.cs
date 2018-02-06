using DQGJK.Models;
using DQGJK.Web.Contexts;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Utils;

namespace DQGJK.Web.Controllers
{
    public class LoginController : Controller
    {
        private DBContext _context;

        public LoginController(DBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public object signIn(string account, string pwd, string remeberMe)
        {
            string returnUrl = string.Empty;

            string _pass = StringUtil.Md5Encrypt(pwd);

            Guser user = _context.Guser.Where(q => q.Account.Equals(account) && q.PassWord.Equals(_pass)).FirstOrDefault();

            if (user == null) { return Json(new { code = -1, msg = "用户名或密码错误" }); }

            if (user.Status == Status.disable) { return Json(new { code = -2, msg = "此用户已禁用，请联系管理员" }); }

            if (!string.IsNullOrEmpty(user.DeptID))
            {
                Department dept = _context.Department.Where(q => q.ID.Equals(user.DeptID)).FirstOrDefault();

                if (dept == null) { return Json(new { code = -3, msg = "抱歉，未找到此用户所属的部门" }); }

                if (dept.Status == Status.disable) { return Json(new { code = -4, msg = "此用户所属部门已禁用，请联系管理员" }); }

                Department dw = _context.Department.Where(q => q.ID.Equals(dept.ParentID)).FirstOrDefault();

                if (dw == null) { return Json(new { code = -5, msg = "抱歉，未找到此用户所属的单位" }); }

                if (dw.Status == Status.disable) { return Json(new { code = -6, msg = "此用户所属单位已禁用，请联系管理员" }); }

                HttpContext.Session.Set("SESSION-DEPARTMENT-KEY", dw);
            }

            HttpContext.Session.Set("SESSION-ACCOUNT-KEY", user);

            //暂时设置为不自动登录
            //remeberMe = "1";
            //if (!string.IsNullOrEmpty(remeberMe))
            //{
            //    HttpCookie cookie = new HttpCookie("session-cookie-name");
            //    cookie["cookie-account-id-key"] = UserContext.user.ID;
            //    cookie.Expires = DateTime.Now.AddDays(7);
            //    System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            //}

            //returnUrl = SiteUtils.isMobile() ? "MobileHome" : "NewImage";

            return Json(new { code = 1, msg = "登录成功", url = "Home" });
        }

        public ActionResult LogOff()
        {
            //Session.Clear();
            //HttpCookie cookie = new HttpCookie("session-cookie-name");
            //cookie.Expires = DateTime.Now.AddDays(-1);
            //System.Web.HttpContext.Current.Response.Cookies.Add(cookie);

            return RedirectToAction("Index", "login");
        }
    }
}