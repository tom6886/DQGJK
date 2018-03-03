using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQGJK.Message.Entity
{
    public class B2Element
    {
        public B2Element(Element element)
        {
            Code = element.Code;
            HumidityLimit = element.HumidityLimit;
            TemperatureLimit = element.TemperatureLimit;
        }

        /// <summary>
        /// 主从机地址
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 湿度阈值
        /// </summary>
        public decimal HumidityLimit { get; set; }

        /// <summary>
        /// 温度阈值
        /// </summary>
        public decimal TemperatureLimit { get; set; }
    }
}
