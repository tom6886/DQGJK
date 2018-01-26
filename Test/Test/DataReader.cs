using System;
using System.Text;

namespace Test
{
    internal class DataReader
    {
        private static byte Start = 0x7E;

        private byte BodyStart = 0x02;

        private byte BodyEnd = 0x16;

        private string DateTimePattern = "20{0}-{1}-{2} {3}:{4}:{5}";

        public enum ReaderState
        {
            Success = 1,
            WrongBodyStart = -2,
            WrongBodyEnd = -3
        }

        #region 属性
        internal ReaderState State { get; private set; }

        internal byte CenterIP { get; private set; }

        internal byte[] ClientIP { get; private set; }

        internal DateTime SendTime { get; private set; }

        internal int Serial { get; private set; }

        internal string FunctionCode { get; private set; }

        internal int DataLength { get; private set; }

        internal byte[] Body { get; private set; }

        internal int TotalLength { get { return DataLength + 23; } }
        #endregion

        internal DataReader(byte[] data)
        {
            //获取中心站位置,一个字节
            byte[] subStart = SubBytes(data, 2);

            CenterIP = subStart[0];

            //获取遥测站位置,五个字节
            byte[] subCenter = SubBytes(subStart, 1);

            ClientIP = new byte[5];

            Array.Copy(subCenter, ClientIP, 5);

            //获取信息发送时间,六个字节,发包时间,BCD码
            byte[] subClient = SubBytes(subCenter, 5);

            SendTime = GetSendTime(subClient);

            //获取流水号,两个字节
            byte[] subTime = SubBytes(subClient, 6);

            Serial = GetSerial(subTime);

            //获取功能码,一个字节
            //F2H 数据链路报（心跳包）
            //B0H 中心站召测设备实时数据参数
            //C0H 终端机自报数据
            //B1H 中心站遥控设备
            //B2H 修改终端机参数
            byte[] subSerial = SubBytes(subTime, 2);

            FunctionCode = subSerial[0].ToString("X2");

            //获取上下行标识及报文长度
            //分两个字节,前一个字节代表发送方,无需解析,后一个字节代表数据长度
            byte[] subCode = SubBytes(subSerial, 1);

            DataLength = Convert.ToInt16(subCode[1]);

            //获取正文数据,02开头,后面的长度等于前面解析的报文长度
            //因为前面的数据宽度都是固定的,若正文不是以02开头,则说明解析有误
            byte[] subLength = SubBytes(subCode, 2);

            if (!subLength[0].Equals(BodyStart)) { State = ReaderState.WrongBodyStart; return; }

            Body = new byte[DataLength];

            Array.Copy(subLength, 1, Body, 0, DataLength);

            //获取数据终止符,若不等于16,则说明解析正文有误
            byte[] subBody = SubBytes(subLength, DataLength + 1);

            if (!subBody[0].Equals(BodyEnd)) { State = ReaderState.WrongBodyEnd; return; }
        }

        internal static int GetDataLength(byte[] data, out int headLength)
        {
            headLength = GetStartPosition(data);

            return Convert.ToInt16(data[18]) + 23;
        }

        #region 操作数据
        private static int GetStartPosition(byte[] data)
        {
            int index = -1;

            for (int i = 0, length = data.Length; i < length; i++)
            {
                //连续两个7E代表报文起始符
                if (data[i] == Start && data[i + 1] == Start)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        private DateTime GetSendTime(byte[] data)
        {
            string[] strs = new string[6];
            StringBuilder sb = new StringBuilder(2);

            //默认取前六个字节 举例：16 12 12 08 59 59
            for (int i = 0; i < 6; i++)
            {
                sb.Append(data[i] >> 4);
                sb.Append(data[i] & 0x0f);
                strs[i] = sb.ToString();
                sb.Clear();
            }

            string timeStr = string.Format(DateTimePattern, strs);

            return Convert.ToDateTime(timeStr);
        }

        private int GetSerial(byte[] data)
        {
            byte[] copy = new byte[2];

            Array.Copy(data, copy, 2);

            return BitConverter.ToInt16(copy, 0);
        }
        #endregion

        private byte[] SubBytes(byte[] data, int start)
        {
            byte[] copy = new byte[data.Length - start];

            Array.Copy(data, start, copy, 0, copy.Length);

            return copy;
        }

        private byte[] SubBytes(byte[] data, int start, int length)
        {
            byte[] copy = new byte[length];

            Array.Copy(data, start, copy, 0, length);

            return copy;
        }
    }
}
