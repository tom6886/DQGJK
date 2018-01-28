using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class Element
    {
        /// <summary>
        /// 主从机地址
        /// </summary>
        internal string Code { get; set; }

        /// <summary>
        /// 湿度
        /// </summary>
        internal decimal Humidity { get; set; }

        /// <summary>
        /// 温度
        /// </summary>
        internal decimal Temperature { get; set; }

        /// <summary>
        /// 继电器1输出状态
        /// </summary>
        internal int RelayOne { get; set; }

        /// <summary>
        /// 继电器2输出状态
        /// </summary>
        internal int RelayTwo { get; set; }

        /// <summary>
        /// 湿度超限报警状态
        /// </summary>
        internal int HumidityAlarm { get; set; }

        /// <summary>
        /// 温度超限报警状态
        /// </summary>
        internal int TemperatureAlarm { get; set; }

        /// <summary>
        /// 除湿状态
        /// </summary>
        internal int Dehumidify { get; set; }

        /// <summary>
        /// 构造函数返回根据数组解析出的要素
        /// 要求第一个元素必须是从机地址
        /// 解析到下一个从机地址标识符为止
        /// </summary>
        /// <param name="data"></param>
        internal Element(byte[] data)
        {
            DecodeResult<string> r = ElementDecode.CodeDecode(data);
            Code = r.Result;

        }
    }
}
