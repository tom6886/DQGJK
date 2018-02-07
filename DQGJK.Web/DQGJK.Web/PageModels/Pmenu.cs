using System.Collections.Generic;

namespace DQGJK.Web.PageModels
{
    public class Pmenu
    {
        public string Name { get; set; }

        public string Href { get; set; }

        public string Iclass { get; set; }

        public string SubMenuStyle { get; set; }

        public Dictionary<string, string> SubMenus { get; set; }

    }
}