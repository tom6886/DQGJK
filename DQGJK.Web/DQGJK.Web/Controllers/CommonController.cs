using DQGJK.Models;
using DQGJK.Web.Contexts;
using DQGJK.Web.PageModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DQGJK.Web.Controllers
{
    public class CommonController : Controller
    {
        private DBContext _context;

        private IHostingEnvironment _host;

        private IDistributedCache _memoryCache;

        public CommonController(DBContext context, IHostingEnvironment host, IDistributedCache memoryCache)
        {
            _context = context;
            _host = host;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public JsonResult GetDept(int deptType, string pId, string key, int page = 1)
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
        public JsonResult GetStation(string key, int areaType, string areaCode, int page = 1)
        {
            Department department = HttpContext.Session.Get<Department>("SESSION-DEPARTMENT-KEY");

            var query = _context.Station.AsQueryable();

            query = query.Where(q => q.Status == Status.enable);

            switch (areaType)
            {
                case 1:
                    string start = areaCode.Substring(0, 2);
                    query = query.Where(q => q.CityCode.StartsWith(start));
                    break;
                case 2:
                    List<Area> countries = _memoryCache.Get<List<Area>>("Country");
                    List<string> codes = countries.Where(q => q.ParentId.Equals(areaCode)).Select(q => q.CityCode).ToList();
                    query = query.Where(q => codes.Contains(q.CityCode));
                    break;
                case 3:
                    query = query.Where(q => q.CityCode.Equals(areaCode));
                    break;
            }

            if (department != null) { query = query.Where(q => q.DeptID.Equals(department.ID)); }

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
        public JsonResult GetRoles(string key, int page = 1)
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
        public JsonResult GetUser(string pId, string key, int page = 1)
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

        [HttpGet]
        public JsonResult GetProvince(string key, int page = 1)
        {
            List<Area> provinces = _memoryCache.Get<List<Area>>("Province");

            if (!string.IsNullOrEmpty(key)) { provinces = provinces.Where(q => q.Name.Contains(key) || q.Pinyin.Contains(key)).ToList(); }

            int total = provinces.Count;

            provinces = provinces.OrderBy(q => q.CityCode).Skip((page - 1) * 10).Take(10).ToList();

            ArrayList results = new ArrayList();

            foreach (var item in provinces)
            {
                results.Add(new { id = item.ID, name = item.Name, code = item.CityCode });
            }

            return Json(new { results = results, total = total, pageSize = 10 });
        }

        [HttpGet]
        public JsonResult GetCity(string pId, string key, int page = 1)
        {
            List<Area> provinces = _memoryCache.Get<List<Area>>("Province");

            List<Area> cities = _memoryCache.Get<List<Area>>("City");

            if (!string.IsNullOrEmpty(pId)) { cities = cities.Where(q => q.ParentId.Equals(pId)).ToList(); }

            if (!string.IsNullOrEmpty(key)) { cities = cities.Where(q => q.Name.Contains(key) || q.Pinyin.Contains(key)).ToList(); }

            int total = cities.Count;

            cities = cities.OrderBy(q => q.CityCode).Skip((page - 1) * 10).Take(10).ToList();

            ArrayList results = new ArrayList();

            foreach (var item in cities)
            {
                Area p = provinces.Where(q => q.ID.Equals(item.ParentId)).FirstOrDefault();

                object pItem = null;

                if (p != null) { pItem = new { id = p.ID, name = p.Name, code = p.CityCode }; }

                results.Add(new { id = item.ID, name = item.Name, code = item.CityCode, province = pItem });
            }

            return Json(new { results = results, total = total, pageSize = 10 });
        }

        [HttpGet]
        public JsonResult GetCountry(string pId, string key, int page = 1)
        {
            List<Area> provinces = _memoryCache.Get<List<Area>>("Province");

            List<Area> cities = _memoryCache.Get<List<Area>>("City");

            List<Area> countries = _memoryCache.Get<List<Area>>("Country");

            if (!string.IsNullOrEmpty(pId)) { countries = countries.Where(q => q.ParentId.Equals(pId)).ToList(); }

            if (!string.IsNullOrEmpty(key)) { countries = countries.Where(q => q.Name.Contains(key) || q.Pinyin.Contains(key)).ToList(); }

            int total = countries.Count;

            countries = countries.OrderBy(q => q.CityCode).Skip((page - 1) * 10).Take(10).ToList();

            ArrayList results = new ArrayList();

            foreach (var item in countries)
            {
                Area ci = cities.Where(q => q.ID.Equals(item.ParentId)).FirstOrDefault();

                Area p = provinces.Where(q => q.ID.Equals(ci?.ParentId)).FirstOrDefault();

                object ciItem = null;

                object pItem = null;

                if (ci != null) { ciItem = new { id = ci.ID, name = ci.Name, code = ci.CityCode }; }

                if (p != null) { pItem = new { id = p.ID, name = p.Name, code = p.CityCode }; }

                results.Add(new { id = item.ID, name = item.Name, code = item.CityCode, province = pItem, city = ciItem });
            }

            return Json(new { results = results, total = total, pageSize = 10 });
        }

        [HttpGet]
        public JsonResult GetBoardConfigs()
        {
            using (XmlReader reader = XmlReader.Create(_host.ContentRootPath + "/Xmls/Boards.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Pboard>));
                List<Pboard> pboards = serializer.Deserialize(reader) as List<Pboard>;
                reader.Close();
                return Json(pboards);
            }
        }

        [HttpGet]
        public string SetBoardConfigs()
        {
            List<Pboard> list = new List<Pboard>();
            Pboard pboard = new Pboard();
            BoardConfig boardConfig = new BoardConfig();
            List<PlotBand> bands = new List<PlotBand>();
            PlotBand plotBand = new PlotBand();
            plotBand.From = -20;
            plotBand.To = 120;
            plotBand.Color = "#55BF3B";
            bands.Add(plotBand);
            boardConfig.Min = -20;
            boardConfig.Max = 200;
            boardConfig.Title = "℃";
            boardConfig.PlotBands = bands;
            pboard.Name = "thermograph";
            pboard.Config = boardConfig;
            list.Add(pboard);

            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Pboard>));
                serializer.Serialize(sw, list);
                sw.Close();
                string str = sw.ToString();

                return str;
            }
        }
    }
}