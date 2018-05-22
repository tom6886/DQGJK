using System;
using System.Text;

namespace DQGJK.Message
{
    public class MessageDecode
    {
        public byte[] Data;

        public bool IsChecked = true;

        private const byte Start = 0x7E;

        private const byte BodyStart = 0x02;

        private const byte BodyEnd = 0x16;

        private const string DateTimePattern = "20{0}-{1}-{2} {3}:{4}:{5}";
        public MessageDecode(byte[] data)
        {
            Data = data;
        }

        #region 解析方法
        //获取中心站位置,一个字节,客户端发送3位,服务端发送9位
        public byte CenterCode(int position)
        {
            return Data[position];
        }

        //获取遥测站位置,六个字节,客户端发送4-9位，服务端发送3-8位
        public byte[] ClientCode(int position)
        {
            byte[] client = new byte[6];

            Array.Copy(Data, position, client, 0, 6);

            return client;
        }

        //获取信息发送时间,六个字节,发包时间,BCD码,10-15位
        //实际应用中有可能传送的时间字符串错误，这种情况返回当前时间，不做特殊处理
        public DateTime SendTime(int position)
        {
            try
            {
                string[] strs = new string[6];
                StringBuilder sb = new StringBuilder(2);

                //默认取前六个字节 举例：16 12 12 08 59 59
                for (int i = position; i < position + 6; i++)
                {
                    sb.Append(Data[i] >> 4);
                    sb.Append(Data[i] & 0x0f);
                    strs[i - position] = sb.ToString();
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

        //获取流水号,两个字节,16-17位
        public int Serial(int position)
        {
            byte[] serial = new byte[2];

            Array.Copy(Data, position, serial, 0, 2);

            return BitConverter.ToInt16(serial, 0);
        }

        //获取功能码,一个字节,18位
        //F2H 数据链路报（心跳包）
        //B0H 中心站召测设备实时数据参数
        //C0H 终端机自报数据
        //B1H 中心站遥控设备
        //B2H 修改终端机参数
        public string FunctionCode(int position)
        {
            return Data[position].ToString("X2");
        }

        //获取上下行标识及报文长度
        //分两个字节,代表数据长度 19-20位
        public int DataLength(int position)
        {
            byte[] length = new byte[2];

            Array.Copy(Data, position, length, 0, 2);

            string lengthStr = BCDUtil.ConvertToHex(length);

            return Convert.ToInt32(lengthStr.Substring(1), 16);
        }

        //获取正文数据,02开头,后面的长度等于前面解析得到的报文长度
        //因为前面的数据宽度都是固定的,若正文不是以02开头,则说明解析有误
        public byte[] Body(int position, int length)
        {
            if (!Data[position].Equals(BodyStart)) { IsChecked = false; return null; }

            byte[] body = new byte[length];

            Array.Copy(Data, position + 1, body, 0, length);

            return body;
        }

        //解析召测/自报数据采集时间，E0E0开头，若不是，则解析有误
        public DateTime DataTime(byte[] data)
        {
            if (data.Length < 2 || !(data[0].Equals(0xE0) && data[1].Equals(0xE0))) { IsChecked = false; return DateTime.MinValue; }

            return ElementDecodeFunctions.DataTime(data);
        }

        public byte[] CRC(int endPosition)
        {
            if (endPosition > Data.Length || !Data[endPosition].Equals(BodyEnd)) { IsChecked = false; return null; }

            byte[] crc = new byte[2];

            Array.Copy(Data, endPosition + 1, crc, 0, 2);

            return crc;
        }

        public bool CheckCRC(byte[] crc, int endBeforeLength)
        {
            byte[] checkData = new byte[endBeforeLength];

            Array.Copy(Data, 0, checkData, 0, endBeforeLength);

            string str = BytesUtil.ToHexString(CRCUtil.CRC16(checkData));

            return str.Equals(BytesUtil.ToHexString(crc));
        }
        #endregion

        private static int GetStartPosition(byte[] data)
        {
            int index = data.Length;

            if (data.Length < 2) { return index; }

            for (int i = 0, length = data.Length; i < length - 1; i++)
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

        public static int GetDataLength(byte[] data, out int headLength)
        {
            headLength = GetStartPosition(data);

            if (data.Length < headLength + 19) { return headLength + 43; }

            byte[] length = new byte[2];

            Array.Copy(data, headLength + 18, length, 0, 2);

            string lengthStr = BCDUtil.ConvertToHex(length);

            return Convert.ToInt32(lengthStr.Substring(1), 16) + 24;
        }
    }
}
