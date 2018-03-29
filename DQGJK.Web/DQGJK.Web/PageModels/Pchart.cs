using System.Collections;

namespace DQGJK.Web.PageModels
{
    public class Pchart
    {
        public Pchart(string code)
        {
            this.Code = code;
            this.XAxis = new ArrayList();
            this.Temperature = new ArrayList();
            this.Humidity = new ArrayList();
        }

        public string Code { get; set; }

        public ArrayList XAxis { get; set; }

        public ArrayList Temperature { get; set; }

        public ArrayList Humidity { get; set; }
    }
}
