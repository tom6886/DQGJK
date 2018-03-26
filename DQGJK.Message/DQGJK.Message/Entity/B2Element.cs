using System;
using System.Collections.Generic;

namespace DQGJK.Message
{
    public class B2Element : IElement
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
        public double HumidityLimit { get; set; }

        /// <summary>
        /// 温度阈值
        /// </summary>
        public double TemperatureLimit { get; set; }

        public byte[] ToByte()
        {
            List<byte> list = new List<byte>();

            list.AddRange(new byte[] { 0x08, 0x08 });
            list.AddRange(BytesUtil.ToHexArray(Code));
            list.AddRange(new byte[] { 0x04, 0x1A });
            string _humLimit = (Math.Round(HumidityLimit, 2) * 100).ToString().PadLeft(6, '0');
            list.AddRange(BCDUtil.ConvertFrom(_humLimit, 3));
            list.AddRange(new byte[] { 0x05, 0x1A });
            string _temLimit = (Math.Round(TemperatureLimit, 2) * 100).ToString().PadLeft(6, '0');
            list.AddRange(BCDUtil.ConvertFrom(_temLimit, 3));

            return list.ToArray();
        }
    }
}
