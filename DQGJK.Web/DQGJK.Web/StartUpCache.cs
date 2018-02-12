using DQGJK.Web.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DQGJK.Web
{
    public class StartUpCache
    {
        private IDistributedCache _memoryCache;

        private IHostingEnvironment _host;

        public StartUpCache(IDistributedCache memoryCache, IHostingEnvironment host)
        {
            _memoryCache = memoryCache;
            _host = host;
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
    }
}
