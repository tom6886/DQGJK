using DQGJK.Models;
using DQGJK.Web.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DQGJK.Web
{
    public class StartUpCache
    {
        private IDistributedCache _memoryCache;

        private IHostingEnvironment _host;

        private DBContext _context;

        public StartUpCache(IDistributedCache memoryCache, IHostingEnvironment host, DBContext context)
        {
            _memoryCache = memoryCache;
            _host = host;
            _context = context;
        }

        public void CacheAll()
        {
            this.RoleCache();
            this.AreaCache();
        }

        public void RoleCache()
        {
            XDocument doc = XDocument.Load(_host.ContentRootPath + "/Xmls/Roles.xml");
            Dictionary<string, string> roles = new Dictionary<string, string>();
            foreach (XElement ele in doc.Root.Elements("role"))
            {
                roles.Add(ele.Element("name").Value, ele.Element("value").Value);
            }

            _memoryCache.Set("Roles", roles);
        }

        public void AreaCache()
        {
            List<Area> areas = _context.Area.ToList();

            _memoryCache.Set("Province", areas.Where(q => q.LevelType == (int)AreaLevelType.Province).ToList());
            _memoryCache.Set("City", areas.Where(q => q.LevelType == (int)AreaLevelType.City).ToList());
            _memoryCache.Set("Country", areas.Where(q => q.LevelType == (int)AreaLevelType.Country).ToList());
        }
    }
}
