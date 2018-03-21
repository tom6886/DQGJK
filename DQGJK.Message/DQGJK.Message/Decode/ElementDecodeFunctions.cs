using System;
using System.Collections.Generic;
using System.Text;

namespace DQGJK.Message
{
    /// <summary>
    /// 1、N(D,d)表示十进制浮点数。其中D表示除小数点以外的数据位数，d表示小数点后的数据位数
    /// 2、例如：02 1A表示实时温度标识符，其中1A位数据结构定义，高5位=3（字节个数），低3位=2（小数点位数）
    /// 3、显示均为BCD码展示。
    /// </summary>
    public class ElementDecodeFunctions
    {
        private const string DateTimePattern = "20{0}-{1}-{2} {3}:{4}:00";

        /// <summary>
        /// E0H	N(10) E0H		
        /// 观测时间，数据定义固定E0H，BCD码，5字节
        /// 示例：E0 E0 16 12 12 08 59
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DateTime DataTime(byte[] data)
        {
            try
            {
                byte[] temp = BytesUtil.SubBytes(data, 2, 5);

                string[] strs = new string[6];
                StringBuilder sb = new StringBuilder(2);

                for (int i = 0; i < 5; i++)
                {
                    sb.Append(temp[i] >> 4);
                    sb.Append(temp[i] & 0x0f);
                    strs[i] = sb.ToString();
                    sb.Clear();
                }

                string timeStr = string.Format(DateTimePattern, strs);

                return Convert.ToDateTime(timeStr);
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// 08H	N(2) 08H
        /// 主从机地址，1字节（其中地址FF表示主机）
        /// 示例：08 08 FF
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Code(byte[] data)
        {
            return BCDUtil.ConvertToHex(data[2]);
        }

        /// <summary>
        /// 湿度 01H	 N(5,2)	1AH 百分比
        /// 温度 02H	 N(5,2)	1AH	摄氏度
        /// 湿度阈值 04H	 N(5,2)	1AH	百分比
        /// 温度阈值 05H	 N(5,2)	1AH	摄氏度
        /// 温湿度/温湿度阈值，BCD码，3字节
        /// 示例：01 1A 00 88 66 02 1A 00 12 34
        ///      04 1A 00 55 66 05 1A 00 66 55
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static double Humiture(byte[] data)
        {
            byte[] temp = BytesUtil.SubBytes(data, 2, 3);

            string bcdStr = BCDUtil.ConvertTo(temp);

            return Convert.ToInt64(bcdStr) / 100.0;
        }

        /// <summary>
        /// 03H	N(4) 10H
        /// 从机状态 2字节
        /// 示例：03 10 00 1F
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DeviceState State(byte[] data)
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

        /// <summary>
        /// F0H	N(2) 08H
        /// 中心站地址，BCD码，1字节
        /// 示例：F0 08 01
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte CenterCode(byte[] data)
        {
            return data[2];
        }

        /// <summary>
        /// F1H	N(12) 30H
        /// 遥测站地址，BCD码，6字节
        /// 示例：F1 28 00 12 34 56 78
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ClientCode(byte[] data)
        {
            byte[] client = new byte[6];

            Array.Copy(data, 2, client, 0, 6);

            return BCDUtil.ConvertTo(client);
        }

        /// <summary>
        /// F2H	N(18) 48H
        /// 中心站1主信道类型及地址TCP协议，IPV4，BCD码，9字节 255255255255 000000，分别表示IP和端口
        /// 示例：F2 48 06 01 23 45 67 89 00 60 12 结果 IP：060.123.456.789，端口：006012
        /// </summary>
        /// <returns></returns>
        public static string IPPort(byte[] data)
        {
            byte[] ipPort = new byte[9];

            Array.Copy(data, 2, ipPort, 0, 9);

            return BCDUtil.ConvertTo(ipPort);
        }

        /// <summary>
        /// F3H	N(4) 10H
        /// min	上报时间间隔，BCD码，2字节
        /// 示例：F3 10 00 60
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int Interval(byte[] data)
        {
            byte[] interval = new byte[2];

            Array.Copy(data, 2, interval, 0, 2);

            return Convert.ToInt16(BCDUtil.ConvertTo(interval));
        }
    }
}
