using System;
using System.Collections.Generic;

namespace Test
{
    /// <summary>
    /// 1、N(D,d)表示十进制浮点数。其中D表示除小数点以外的数据位数，d表示小数点后的数据位数
    /// 2、例如：02 1A表示实时温度标识符，其中1A位数据结构定义，高5位=3（字节个数），低3位=2（小数点位数）
    /// 3、显示均为BCD码展示。
    /// </summary>
    internal class ElementDecodeFunctions
    {
        /// <summary>
        /// 08H	N(2) 08H
        /// 主从机地址，1字节（其中地址FF表示主机）
        /// 示例：08 08 FF
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static string Code(byte[] data)
        {
            return BCDConverter.ConvertTo(data[2]);
        }

        /// <summary>
        /// 湿度 01H	 N(5,2)	1AH 百分比
        /// 温度 02H	 N(5,2)	1AH	摄氏度
        /// 温湿度，BCD码，3字节
        /// 示例：01 1A 00 88 66 02 1A 00 12 34
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static decimal Humiture(byte[] data)
        {
            byte[] temp = BytesUtil.SubBytes(data, 2, 3);

            string bcdStr = BCDConverter.ConvertTo(temp);

            return Convert.ToInt16(bcdStr) / 100.0m;
        }

        /// <summary>
        /// 03H	N(4) 10H
        /// 从机状态 2字节
        /// 示例：03 10 00 1F
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static DeviceState State(byte[] data)
        {
            byte[] temp = BytesUtil.SubBytes(data, 2, 2);

            int[] arr = BytesUtil.ToBinaryArray(temp);

            if (arr == null) { return null; }

            Array.Reverse(arr);

            DeviceState state = new DeviceState()
            {
                RelayOne = arr[0],
                RelayTwo = arr[1],
                HumidityAlarm = arr[2],
                TemperatureAlarm = arr[3],
                Dehumidify = arr[4]
            };

            return state;
        }
    }
}
